using CrossProject.Core.Conditions;
using CrossProject.Core.Quests;
using CrossProject.Core.SaveLoad;

namespace L2Farm.Scripts.Conditions.QuestNotCompleted
{
    public class QuestNotCompletedCondition : Condition<QuestNotCompletedConditionConfig>
    {
        private readonly GameStateManager _gameStateManager;

        public QuestNotCompletedCondition(
            GameStateManager gameStateManager,
            ConditionService conditionService)
            : base(conditionService)
        {
            _gameStateManager = gameStateManager;
        }

        public override bool Check()
        {
            return _gameStateManager.State.Get<QuestsLogPart>().launchedQuests.ContainsKey(config.questId) == false;
        }
    }
}
