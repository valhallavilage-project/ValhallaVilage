namespace CrossProject.Core
{
    public class NoticeMobTransition : BaseMobTransition
    {
        private readonly INoticeEnemyArea _noticeEnemyArea;
        private readonly IMobPerUpdateData _perUpdateData;
        private readonly MobConfig _config;
        private readonly IRoamArea _roamArea;

        public override MobState State => MobState.Notice;
        public override MobTransition Transition => MobTransition.Notice;

        public NoticeMobTransition(INoticeEnemyArea noticeEnemyArea, IMobPerUpdateData perUpdateData, MobConfig config,
            IRoamArea roamArea)
        {
            _noticeEnemyArea = noticeEnemyArea;
            _perUpdateData = perUpdateData;
            _config = config;
            _roamArea = roamArea;
        }

        protected override bool Condition()
        {
            return _noticeEnemyArea.IsEnemyInsideArea;
        }

        protected override void FillConditionForStates()
        {
            base.FillConditionForStates();

            ConditionForState[MobState.RoamRotation] = RoamRotationCondition;
            ConditionForState[MobState.ReturnToRoamArea] = RoamRotationCondition;
        }

        private bool RoamRotationCondition()
        {
            return _roamArea.IsInside(_perUpdateData.Position)
                ? Condition()
                : (_noticeEnemyArea.Enemy.position - _perUpdateData.Position).magnitude < _config.MinDistanceToApproach;
        }
    }
}