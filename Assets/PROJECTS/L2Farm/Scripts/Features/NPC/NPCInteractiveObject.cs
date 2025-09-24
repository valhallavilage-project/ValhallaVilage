using CrossProject.Core;
using CrossProject.Core.Interactions;
using CrossProject.Core.Quests;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace L2Farm.Features.NPC
{
    public class NPCInteractiveObject : AbstractInteractiveObject
    {
        private QuestService _questService;

        [SerializeField] private NpcQuestMarker _marker;

        private QuestId questId;

        public QuestId CurrentQuestId => questId;

        protected override async UniTask AfterInteraction()
        {
            if (questId != null)
                await _questService.TryProceedStepsOf(questId);
        }

        public void SetQuest(QuestId id, QuestService questService)
        {
            questId = id;
            _questService = questService;
            _questService.OnQuestWin += HandleCurrentQuestWin;
            _marker.Setup(id, _questService);
        }

        private void HandleCurrentQuestWin(QuestId id)
        {
            if (id == questId)
                questId = null;
        }
    }
}
