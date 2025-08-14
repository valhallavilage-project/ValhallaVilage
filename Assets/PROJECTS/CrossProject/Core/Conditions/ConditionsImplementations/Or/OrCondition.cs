using System.Linq;

namespace CrossProject.Core.Conditions.ConditionsImplementations
{
    public class OrCondition : Condition<OrConditionConfig>
    {
        public OrCondition(ConditionService conditionService) : base(conditionService) {}

        public override bool Check()
        {
            return config.conditions.Any(x => conditionService.Check(x));
        }
    }
}
