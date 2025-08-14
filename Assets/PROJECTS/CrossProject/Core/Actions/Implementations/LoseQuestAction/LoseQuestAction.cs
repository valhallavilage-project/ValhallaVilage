using CrossProject.Core.Quests;

namespace CrossProject.Core.Actions.Implementations
{
    public class LoseQuestAction : Action<LoseQuestActionConfig>
    {
        private readonly QuestService _questService;

        public LoseQuestAction(QuestService questService)
        {
            _questService = questService;
        }

        public override void Execute()
        {
            _questService.ForceLose(config.questId);
        }
    }
}
