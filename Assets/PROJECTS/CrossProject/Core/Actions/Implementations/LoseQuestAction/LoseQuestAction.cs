using CrossProject.Core.Quests;
using Cysharp.Threading.Tasks;

namespace CrossProject.Core.Actions.Implementations
{
    public class LoseQuestAction : Action<LoseQuestActionConfig>
    {
        private readonly QuestService _questService;

        public LoseQuestAction(QuestService questService)
        {
            _questService = questService;
        }

        public override async UniTask Execute()
        {
            _questService.ForceLose(config.questId);
        }
    }
}
