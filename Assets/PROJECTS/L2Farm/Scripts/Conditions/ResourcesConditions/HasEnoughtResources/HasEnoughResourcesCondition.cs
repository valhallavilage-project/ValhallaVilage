using CrossProject.Core.Conditions;
using CrossProject.Core.InGameResources;
using CrossProject.Core.SaveLoad;

namespace L2Farm.Scripts.Conditions
{
    public class HasEnoughResourcesCondition : Condition<HasEnoughResourcesConditionConfig>
    {
        private readonly GameStateManager _gameStateManager;

        public HasEnoughResourcesCondition(
            GameStateManager gameStateManager,
            ConditionService conditionService)
            : base(conditionService)
        {
            _gameStateManager = gameStateManager;
        }

        public override bool Check()
        {
            var part = _gameStateManager.State.Get<ResourceContentPart>();

            foreach (var resourceCondition in config.ResourceConditions)
            {
                if (!part.Resources.TryGetValue(resourceCondition.Id, out int count))
                    return false;

                if (count < resourceCondition.NeededAmount)
                    return false;
            }

            return true;
        }
    }
}
