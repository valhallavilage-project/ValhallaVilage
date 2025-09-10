namespace CrossProject.Core
{
    public class ReturnToRoamAreaMobTransition : BaseMobTransition
    {
        private readonly INoticeEnemyArea _noticeEnemyArea;
        private readonly IRoamArea _roamArea;
        private readonly IMobPersistentData _persistentData;
        private readonly IMobPerUpdateData _perUpdateData;
        private readonly MobConfig _config;

        public override MobState State => MobState.ReturnToRoamArea;
        public override MobTransition Transition => MobTransition.ReturnToRoamArea;

        public ReturnToRoamAreaMobTransition(INoticeEnemyArea noticeEnemyArea, IRoamArea roamArea, IMobPersistentData persistentData, IMobPerUpdateData perUpdateData,
            MobConfig config)
        {
            _noticeEnemyArea = noticeEnemyArea;
            _roamArea = roamArea;
            _persistentData = persistentData;
            _perUpdateData = perUpdateData;
            _config = config;
        }

        protected override bool Condition()
        {
            return !_noticeEnemyArea.IsEnemyInsideArea;
        }

        protected override void FillConditionForStates()
        {
            base.FillConditionForStates();

            ConditionForState[MobState.RoamRotation] = () => _persistentData.IsRoamRotationFinished && !_roamArea.IsInside(_perUpdateData.Position);
        }
    }
}