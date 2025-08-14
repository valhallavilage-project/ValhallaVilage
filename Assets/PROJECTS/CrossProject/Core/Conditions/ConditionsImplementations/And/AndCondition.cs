using System.Linq;

namespace CrossProject.Core.Conditions.ConditionsImplementations
{
    public class AndCondition : Condition<AndConditionConfig>
    {
        public AndCondition(ConditionService conditionService) : base(conditionService) {}

        public override bool Check()
        {
            return config.conditions.All(condition => conditionService.Check(condition));
        }
    }
}
