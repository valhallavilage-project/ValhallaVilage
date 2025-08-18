using System.Collections.Generic;
using System.Linq;
using CrossProject.Core.Actions;
using CrossProject.Core.Conditions;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace CrossProject.Core.Quests
{
    public class QuestService : IInitializable
    {
        private readonly AddressablesManager _addressablesManager;
        private readonly GameStateManager _gameStateManager;
        private readonly ConditionService _conditionService;
        private readonly ActionService _actionService;

        private QuestSetConfig _questSetConfig;

        private readonly Dictionary<QuestId, int> _launchedQuests = new();

        public bool IsInitialized { get; private set; }

        public QuestService(
            AddressablesManager addressablesManager,
            GameStateManager gameStateManager,
            ConditionService conditionService,
            ActionService actionService)
        {
            _addressablesManager = addressablesManager;
            _gameStateManager = gameStateManager;
            _conditionService = conditionService;
            _actionService = actionService;
        }

        public async UniTask Initialize()
        {
            _questSetConfig = await _addressablesManager.LoadAssetAsync<QuestSetConfig>();
            LoadQuests();
            IsInitialized = true;
        }

        private void LoadQuests()
        {
            var part = _gameStateManager.State.Get<QuestsLogPart>();

            if (part.launchedQuests.Count != 0)
            {
                foreach (var log in part.launchedQuests)
                {
                    TryLaunch(log.Key, log.Value);
                }
            }
            else
            {
                TryLaunch(new QuestId(_questSetConfig.items.First().id));
            }
        }

        public bool TryLaunch(QuestId id, int step = 0)
        {
            //TODO : VM : queue of non-launched quests
            if (_launchedQuests.ContainsKey(id))
                return false;

            var config = _questSetConfig.Get(id);
            if (!_conditionService.Check(config.launchCondition))
                return false;

            _launchedQuests.Add(id, step);
            _gameStateManager.State.Get<QuestsLogPart>().launchedQuests.Add(id, step);
            _gameStateManager.Save();
            _actionService.Execute(config.launchActions);
            TryProceed(id);
            return true;
        }

        private bool TryWin(QuestId id)
        {
            var config = _questSetConfig.Get(id);
            if (!_conditionService.Check(config.winCondition))
                return false;

            ForceWin(id, config);
            return true;
        }

        private bool TryLose(QuestId id)
        {
            var config = _questSetConfig.Get(id);
            if (!_conditionService.Check(config.loseCondition))
                return false;

            ForceLose(id, config);
            return true;
        }

        public bool TryProceed(QuestId id)
        {
            if (!_launchedQuests.TryGetValue(id, out var stepIndex))
                return false;

            var step = _questSetConfig.Get(id).steps[stepIndex];

            if (_conditionService.Check(step.loseCondition))
            {
                _actionService.Execute(step.loseActions);
                return false;
            }

            if (!_conditionService.Check(step.winCondition))
            {
                return false;
            }

            _actionService.Execute(step.winActions);
            _launchedQuests[id]++;
            return true;
        }

        public void ForceLose(QuestId id, QuestConfig config = null)
        {
            config ??= _questSetConfig.Get(id);

            var part = _gameStateManager.State.Get<QuestsLogPart>();
            part.launchedQuests.Remove(id);
            part.lostQuests.Add(id);
            _gameStateManager.Save();

            _launchedQuests.Remove(id);
            _actionService.Execute(config.loseActions);
        }

        public void ForceWin(QuestId id, QuestConfig config = null)
        {
            config ??= _questSetConfig.Get(id);

            var part = _gameStateManager.State.Get<QuestsLogPart>();
            part.launchedQuests.Remove(id);
            part.wonQuests.Add(id);
            _gameStateManager.Save();

            _launchedQuests.Remove(id);
            _actionService.Execute(config.winActions);
        }

        public QuestConfig GetConfigFor(QuestId id) => _questSetConfig.Get(id);
    }
}
