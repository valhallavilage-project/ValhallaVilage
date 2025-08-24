using CrossProject.Core.Conditions;
using CrossProject.Core.InGameResources;
using CrossProject.Core.Quests;
using CrossProject.Core.SaveLoad;

namespace L2Farm.Scripts.Conditions
{
    public class NotEnoughResourcesCondition : Condition<NotEnoughResourcesConditionConfig>
    {
        private readonly QuestService _questService;
        private readonly GameStateManager _gameStateManager;

        public NotEnoughResourcesCondition(
            QuestService questService,
            GameStateManager gameStateManager,
            ConditionService conditionService)
            : base(conditionService)
        {
            _questService = questService;
            _gameStateManager = gameStateManager;
        }

        public override bool Check()
        {
            var part = _gameStateManager.State.Get<ResourceContentPart>();

            var stepConfig = _questService.GetConfigFor(config.questId).steps[config.stepIndexWithResourceCondition];
            var resourceConditions = ((HasEnoughResourcesConditionConfig)stepConfig.winCondition).resourceConditions;
            
            foreach (var condition in resourceConditions)
            {
                if (!part.Resources.TryGetValue(condition.resourceId, out int count))
                    return true;

                if (count < condition.neededQuantity)
                    return true;
            }

            return false;
        }
    }
}
