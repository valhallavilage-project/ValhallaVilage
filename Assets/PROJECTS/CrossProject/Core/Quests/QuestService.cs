using System.Collections.Generic;
using System.Linq;
using CrossProject.Core.Actions;
using CrossProject.Core.Conditions;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Core.Quests
{
    public class QuestService : IInitializable
    {
        #region Injected fields, private fields, properties and init

        private readonly AddressablesManager _addressablesManager;
        private readonly GameStateManager _gameStateManager;
        private readonly ConditionService _conditionService;
        private readonly ActionService _actionService;

        private QuestSetConfig _questSetConfig;

        private readonly Dictionary<QuestId, int> _launchedQuests = new();

        public bool IsInitialized { get; private set; }

        public event System.Action<QuestId> OnQuestLaunch;
        public event System.Action<QuestId> OnQuestWin;
        public event System.Action<QuestId> OnQuestLose;

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

            var newList = part.launchedQuests.ToList();
            foreach (var log in newList)
                TryLaunch(log.Key, log.Value);
        }

        #endregion

        public bool TryLaunch(QuestId id, int stepIndex = 0)
        {
            if (string.IsNullOrEmpty(id))
            {
                Debug.LogError($"[{nameof(QuestService)}] : Trying to Launch quest with id NULL");
                return false;
            }

            if (_launchedQuests.ContainsKey(id))
                return false;

            //TODO : VM : queue of non-launched, but fired quests?
            var config = _questSetConfig.Get(id);
            if (config == null)
            {
                Debug.LogError($"[{nameof(QuestService)}] : cannot find config for {id}!");
                return false;
            }
            if (!_conditionService.Check(config.launchCondition))
                return false;

            _launchedQuests.Add(id, stepIndex);
            var part = _gameStateManager.State.Get<QuestsLogPart>();
            part.launchedQuests[id] = stepIndex;
            _gameStateManager.Save();
            _actionService.Execute(config.launchActions);
            OnQuestLaunch?.Invoke(id);
            Debug.Log($"[{nameof(QuestService)}] : Launch : {id}");
            if (config.proceedAfterLaunch)
                TryProceedStepsOf(id);
            return true;
        }

        public bool CanProceed(QuestId id)
        {
            Debug.Log($"[{nameof(QuestService)}] : CanProceed : {id}");

            if (!_launchedQuests.TryGetValue(id, out var stepIndex))
            {
                Debug.LogError($"[{nameof(QuestService)}] : {id} is not launched!");
                return false;
            }

            var questConfig = _questSetConfig.Get(id);
            if (stepIndex >= questConfig.steps.Count)
                return true;

            var step = questConfig.steps[stepIndex];
            return _conditionService.Check(step.winCondition);
        }

        public bool TryProceedStepsOf(QuestId id)
        {
            Debug.Log($"[{nameof(QuestService)}] : TryProceed : {id}");

            if (!_launchedQuests.TryGetValue(id, out var stepIndex))
            {
                Debug.LogError($"[{nameof(QuestService)}] : {id} is not launched!");
                return false;
            }

            var questConfig = _questSetConfig.Get(id);
            if (stepIndex >= questConfig.steps.Count)
            {
                ForceWin(id);
                return true;
            }

            var step = questConfig.steps[stepIndex];
            _actionService.Execute(step.stepAction);

            if (_conditionService.Check(step.loseCondition))
            {
                _actionService.Execute(step.loseActions);
                Debug.Log($"[{nameof(QuestService)}] : {id} at step {stepIndex} Lose");
                return false;
            }

            if (!_conditionService.Check(step.winCondition))
            {
                Debug.Log($"[{nameof(QuestService)}] : {id} at step {stepIndex} can't proceed");
                return false;
            }

            _actionService.Execute(step.winActions);
            _launchedQuests[id]++;
            _gameStateManager.State.Get<QuestsLogPart>().launchedQuests[id]++;
            _gameStateManager.Save();
            Debug.Log($"[{nameof(QuestService)}] : Step proceed : {id} : {_launchedQuests[id] - 1} ---> {_launchedQuests[id]}");
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
            OnQuestLose?.Invoke(id);
            Debug.Log($"[{nameof(QuestService)}] : {id} Quest Lose :-(");
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
            OnQuestWin?.Invoke(id);
            Debug.Log($"[{nameof(QuestService)}] : {id} Quest Win :-)");
        }

        public QuestConfig GetConfigFor(QuestId id) => _questSetConfig.Get(id);
    }
}
