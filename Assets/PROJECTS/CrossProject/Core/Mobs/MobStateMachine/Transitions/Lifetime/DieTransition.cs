namespace CrossProject.Core
{
    public class DieTransition : BaseMobTransition
    {
        private readonly IHealthHandler _healthHandler;

        public override MobState State => MobState.Die;
        public override MobTransition Transition => MobTransition.Die;

        public DieTransition(IHealthHandler healthHandler)
        {
            _healthHandler = healthHandler;
        }

        protected override bool Condition()
        {
            return _healthHandler.Health.Value <= 0;
        }
    }
}
