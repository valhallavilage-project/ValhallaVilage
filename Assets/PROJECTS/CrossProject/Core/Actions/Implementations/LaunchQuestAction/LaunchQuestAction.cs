using CrossProject.Core.Quests;
using Cysharp.Threading.Tasks;

namespace CrossProject.Core.Actions.Implementations
{
    public class LaunchQuestAction : Action<LaunchQuestActionConfig>
    {
        private readonly QuestService _questService;

        public LaunchQuestAction(QuestService questService)
        {
            _questService = questService;
        }

        public override async UniTask Execute()
        {
            await _questService.TryLaunch(config.questId);
        }
    }
}
