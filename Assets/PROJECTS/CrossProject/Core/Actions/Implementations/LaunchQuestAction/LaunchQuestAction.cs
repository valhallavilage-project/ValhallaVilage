using CrossProject.Core.Quests;

namespace CrossProject.Core.Actions.Implementations
{
    public class LaunchQuestAction : Action<LaunchQuestActionConfig>
    {
        private readonly QuestService _questService;

        public LaunchQuestAction(QuestService questService)
        {
            _questService = questService;
        }

        public override void Execute()
        {
            _questService.TryLaunch(config.questId);
        }
    }
}
