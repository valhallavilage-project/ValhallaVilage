using CrossProject.Core.Conditions;

namespace L2Farm.Scripts.Conditions
{
    public class HasEnoughResourcesCondition : Condition<HasEnoughResourcesConditionConfig>
    {
        public HasEnoughResourcesCondition(ConditionService conditionService) : base(conditionService) {}

        public override bool Check()
        {
            throw new System.NotImplementedException();
        }
    }
}
