using CrossProject.Core;
using CrossProject.Core.Interactions;
using CrossProject.Core.Quests;
using Cysharp.Threading.Tasks;
using VContainer;

namespace L2Farm.Features.NPC
{
    public class NPCInteractiveObject : AbstractInteractiveObject
    {
        private QuestService _questService;

        private QuestId questId;

        public QuestId CurrentQuestId => questId;

        private void Start()
        {
            ManualPrefabInjector.Instance?.Inject(this);
        }

        [Inject]
        private void Construct(QuestService questService)
        {
            _questService = questService;
        }

        protected override async UniTask AfterInteraction()
        {
            if (questId != null)
                _questService.TryProceedStepsOf(questId);
        }

        public void SetQuest(QuestId id)
        {
            questId = id;
        }
    }
}
