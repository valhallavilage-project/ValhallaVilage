namespace CrossProject.Core
{
    public class AttackPauseMobTransition : BaseMobTransition

    {
        public override MobState State => MobState.AttackPause;
        public override MobTransition Transition => MobTransition.AttackPause;
    }
}