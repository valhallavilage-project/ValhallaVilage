namespace CrossProject.Core.Conditions.ConditionsImplementations
{
    public class TrueCondition : Condition<TrueConditionConfig>
    {
        public TrueCondition(ConditionService conditionService) : base(conditionService) {}

        public override bool Check() => true;
    }
}
