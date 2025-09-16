namespace CrossProject.Core
{
    public class ReturnToRoamAreaMobTransition : BaseMobTransition
    {
        private readonly IAgroArea _agroArea;
        private readonly IRoamArea _roamArea;
        private readonly IMobPersistentData _persistentData;
        private readonly IMobPerUpdateData _perUpdateData;

        public override MobState State => MobState.ReturnToRoamArea;
        public override MobTransition Transition => MobTransition.ReturnToRoamArea;

        public ReturnToRoamAreaMobTransition(IAgroArea agroArea, IRoamArea roamArea, IMobPersistentData persistentData, IMobPerUpdateData perUpdateData)
        {
            _agroArea = agroArea;
            _roamArea = roamArea;
            _persistentData = persistentData;
            _perUpdateData = perUpdateData;
        }

        protected override bool Condition()
        {
            return !_agroArea.IsEnemyInsideArea;
        }

        protected override void FillConditionForStates()
        {
            base.FillConditionForStates();

            ConditionForState[MobState.RoamRotation] = () => _persistentData.IsRoamRotationFinished && !_roamArea.IsInside(_perUpdateData.Position);
        }
    }
}