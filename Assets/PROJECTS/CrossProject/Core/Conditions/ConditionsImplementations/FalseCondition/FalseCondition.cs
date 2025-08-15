namespace CrossProject.Core.Conditions.ConditionsImplementations
{
    public class FalseCondition : Condition<FalseConditionConfig>
    {
        public FalseCondition(ConditionService conditionService) : base(conditionService) {}

        public override bool Check() => false;
    }
}
