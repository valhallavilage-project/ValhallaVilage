using CrossProject.Core.Actions;
using CrossProject.Core.Interactions;
using CrossProject.Core.Quests;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace L2Farm.Features.NPC
{
    public class NPCInteractiveObject : AbstractInteractiveObject
    {
        [SerializeField] private NpcQuestMarker _marker;
        [SerializeField] private AdditionalNpcActions _additionalNpcActions;
        
        private QuestService _questService;
        private QuestId _questId;
        private ActionService _actionService;

        public QuestId CurrentQuestId => _questId;

        protected override async UniTask AfterInteraction()
        {
            if (_questId != null)
                await _questService.TryProceedStepsOf(_questId);

            if (_additionalNpcActions != null)
            {
                await _additionalNpcActions.Launch(_actionService);
            }
        }

        public void SetQuest(QuestId id, QuestService questService, ActionService actionService)
        {
            _actionService = actionService;
            _questId = id;
            _questService = questService;
            _questService.OnQuestWin += HandleCurrentQuestWin;
            _marker.Setup(id, _questService);
        }

        private void HandleCurrentQuestWin(QuestId id)
        {
            if (id == _questId)
                _questId = null;
        }
    }
}
