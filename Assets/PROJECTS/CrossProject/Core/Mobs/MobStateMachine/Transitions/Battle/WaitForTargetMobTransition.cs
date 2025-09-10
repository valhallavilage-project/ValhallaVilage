namespace CrossProject.Core
{
    public class WaitForTargetMobTransition : BaseMobTransition
    {
        private readonly INoticeEnemyArea _noticeEnemyArea;
        private readonly IMobPerUpdateData _perUpdateData;
        private readonly MobConfig _config;
        private readonly IMobPersistentData _persistentData;

        public override MobState State => MobState.WaitForTarget;
        public override MobTransition Transition => MobTransition.WaitForTarget;

        public WaitForTargetMobTransition(INoticeEnemyArea noticeEnemyArea, IMobPerUpdateData perUpdateData, MobConfig config,
            IMobPersistentData persistentData)
        {
            _noticeEnemyArea = noticeEnemyArea;
            _perUpdateData = perUpdateData;
            _config = config;
            _persistentData = persistentData;
        }

        protected override bool Condition()
        {
            return !_noticeEnemyArea.IsEnemyInsideArea;
        }
        
        protected override void FillConditionForStates()
        {
            base.FillConditionForStates();
           
            ConditionForState[MobState.ApproachTarget] = () => Condition() && (_noticeEnemyArea.Enemy.position - _perUpdateData.Position).magnitude > _config.MinDistanceToApproach;
            ConditionForState[MobState.RotateToTarget] = () => Condition() && _persistentData.IsRotationToTargetFinished && (_noticeEnemyArea.Enemy.position - _perUpdateData.Position).magnitude > _config.MinDistanceToApproach;
        }
    }
}