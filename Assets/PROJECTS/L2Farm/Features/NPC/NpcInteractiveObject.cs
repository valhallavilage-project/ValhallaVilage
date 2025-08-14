using CrossProject.Core;
using CrossProject.Core.Interactions;
using CrossProject.Core.Quests;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using VContainer;

namespace L2Farm.Features.NPC
{
    public class NpcInteractiveObject : InteractiveObject
    {
        private UiService _uiService;
        private QuestService _questService;

        public QuestId questId;

        [Inject]
        private void Construct(
            UiService uiService,
            QuestService questService)
        {
            _uiService = uiService;
            _questService = questService;
        }

        private void Start()
        {
            ManualPrefabInjector.Instance.Inject(this);
        }

        public override async UniTask Interaction()
        {
            //_uiService.TryOpen<>();
        }
    }
}