namespace CrossProject.Core
{
    public class WaitForTargetMobTransition : BaseMobTransition
    {
        private readonly IAgroArea _agroArea;
        private readonly IMobPerUpdateData _perUpdateData;
        private readonly MobConfig _config;
        private readonly IMobPersistentData _persistentData;

        public override MobState State => MobState.WaitForTarget;
        public override MobTransition Transition => MobTransition.WaitForTarget;

        public WaitForTargetMobTransition(IAgroArea agroArea, IMobPerUpdateData perUpdateData, MobConfig config,
            IMobPersistentData persistentData)
        {
            _agroArea = agroArea;
            _perUpdateData = perUpdateData;
            _config = config;
            _persistentData = persistentData;
        }

        protected override bool Condition()
        {
            return !_agroArea.IsEnemyInsideArea;
        }
        
        protected override void FillConditionForStates()
        {
            base.FillConditionForStates();
           
            ConditionForState[MobState.ApproachTarget] = () => Condition() && (_agroArea.Enemy.position - _perUpdateData.Position).magnitude > _config.MinDistanceToApproach;
            ConditionForState[MobState.RotateToTarget] = () => Condition() && _persistentData.IsRotationToTargetFinished && (_agroArea.Enemy.position - _perUpdateData.Position).magnitude > _config.MinDistanceToApproach;
        }
    }
}