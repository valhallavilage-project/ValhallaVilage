namespace CrossProject.Core
{
    public class DeadMobTransition : BaseMobTransition
    {
        public override MobState State => MobState.Dead;
        public override MobTransition Transition => MobTransition.Dead;
    }
}
