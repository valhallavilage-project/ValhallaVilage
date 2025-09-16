namespace CrossProject.Core
{
    public class RotateToTargetMobTransition : BaseMobTransition
    {
        private readonly IAgroArea _agroArea;
        private readonly IMobPerUpdateData _perUpdateData;
        private readonly MobConfig _config;
        private readonly IRoamArea _roamArea;

        public override MobState State => MobState.RotateToTarget;
        public override MobTransition Transition => MobTransition.RotateToTarget;

        public RotateToTargetMobTransition(IAgroArea agroArea, IMobPerUpdateData perUpdateData, MobConfig config,
            IRoamArea roamArea)
        {
            _agroArea = agroArea;
            _perUpdateData = perUpdateData;
            _config = config;
            _roamArea = roamArea;
        }

        protected override bool Condition()
        {
            return _roamArea.IsInside(_perUpdateData.Position)
                ? _agroArea.IsEnemyInsideArea
                : (_agroArea.Enemy.position - _perUpdateData.Position).magnitude < _config.MinDistanceToApproach;
        }
    }
}