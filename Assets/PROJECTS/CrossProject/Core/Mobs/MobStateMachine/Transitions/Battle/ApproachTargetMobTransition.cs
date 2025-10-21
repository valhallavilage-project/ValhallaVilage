namespace CrossProject.Core
{
    public class ApproachTargetMobTransition : BaseMobTransition
    {
        private readonly IMobPersistentData _persistentData;
        private readonly IAgroArea _agroArea;
        private readonly IMobPerUpdateData _perUpdateData;
        private readonly MobConfig _config;
        private readonly IRoamArea _roamArea;

        public override MobState State => MobState.ApproachTarget;
        public override MobTransition Transition => MobTransition.ApproachTarget;

        public ApproachTargetMobTransition(IMobPersistentData persistentData, IAgroArea agroArea,
            IMobPerUpdateData mobPerUpdateData, MobConfig config, IRoamArea roamArea)
        {
            _persistentData = persistentData;
            _agroArea = agroArea;
            _perUpdateData = mobPerUpdateData;
            _config = config;
            _roamArea = roamArea;
        }

        protected override bool Condition()
        {
            return _agroArea.IsEnemyInsideArea;
        }

        protected override void FillConditionForStates()
        {
            base.FillConditionForStates();

            ConditionForState[MobState.RotateToTarget] = RotateToTargetCondition;
            ConditionForState[MobState.AttackPause] = () => Condition() && (_agroArea.Enemy.position - _perUpdateData.Position).magnitude > _config.AttackDistance;
            ConditionForState[MobState.WaitForTarget] = () => !_agroArea.IsEnemyInactive && (_agroArea.Enemy.position - _perUpdateData.Position).magnitude < _config.MinDistanceToApproach;
        }

        private bool RotateToTargetCondition()
        {
            if (_roamArea.IsInside(_perUpdateData.Position))
            {
                return Condition() && _persistentData.IsRotationToTargetFinished;
            }

            return _persistentData.IsRotationToTargetFinished;
        }
    }
}