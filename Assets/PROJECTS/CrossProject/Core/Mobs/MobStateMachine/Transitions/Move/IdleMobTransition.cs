namespace CrossProject.Core
{
    public class IdleMobTransition : BaseMobTransition
    {
        public override MobState State => MobState.Idle;
        public override MobTransition Transition => MobTransition.Idle;
    }
}