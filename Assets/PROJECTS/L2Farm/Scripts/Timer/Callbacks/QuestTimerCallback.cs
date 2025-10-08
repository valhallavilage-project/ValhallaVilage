using CrossProject.Core.Actions;
using CrossProject.Core.Actions.Implementations;
using CrossProject.Core.Quests;
using Cysharp.Threading.Tasks;

namespace L2Farm
{
    public class QuestTimerCallback : ITimerCallback
    {
        private readonly ActionService _actionService;
        private readonly QuestService _questService;
        private readonly QuestId _questId;

        public QuestTimerCallback(QuestId questId, ActionService actionService, QuestService questService)
        {
            _actionService = actionService;
            _questService = questService;
            _questId = questId;
        }

        public async UniTask Execute()
        {
            _questService.OnQuestWin += QuestFinishedCallback;

            await _actionService.Execute(new LaunchQuestActionConfig
            {
                questId = _questId
            });

            var isQuestFinished = false;

            await UniTask.WaitUntil(() => isQuestFinished);

            void QuestFinishedCallback(QuestId q)
            {
                if (q != _questId)
                {
                    return;
                }

                _questService.OnQuestWin -= QuestFinishedCallback;

                isQuestFinished = true;
            }
        }
    }
}