namespace CrossProject.Core
{
    public class ResponsiveNoticeMobTransition : NoticeMobTransition
    {
        private readonly IHealthHandler _healthHandler;

        public override MobState State => MobState.ResponsiveNotice;
        public override MobTransition Transition => MobTransition.ResponsiveNotice;

        public ResponsiveNoticeMobTransition(IAgroArea agroArea, IMobPerUpdateData perUpdateData, MobConfig config,
            IRoamArea roamArea, IHealthHandler healthHandler)
            : base(agroArea, perUpdateData, config, roamArea)
        {
            _healthHandler = healthHandler;
        }

        protected override bool Condition()
        {
            return base.Condition() && _healthHandler.Health.Value < _healthHandler.MaxHealth.Value;
        }
    }
}
