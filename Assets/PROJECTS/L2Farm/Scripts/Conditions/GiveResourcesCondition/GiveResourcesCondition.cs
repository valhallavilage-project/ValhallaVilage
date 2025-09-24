using CrossProject.Core.Conditions;

namespace L2Farm.Scripts
{
    public class GiveResourcesCondition : Condition<GiveResourcesConditionConfig>
    {
        public GiveResourcesCondition(ConditionService conditionService)
            : base(conditionService)
        {
        }

        public override bool Check()
        {
            return true;
        }
    }
}
