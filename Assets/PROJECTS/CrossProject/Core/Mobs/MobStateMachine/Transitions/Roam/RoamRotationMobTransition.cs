namespace CrossProject.Core
{
    public class RoamRotationMobTransition : BaseMobTransition
    {
        private readonly IMobPersistentData _persistentData;
        private readonly IRoamArea _roamArea;
        private readonly IMobPerUpdateData _perUpdateData;

        public override MobState State => MobState.RoamRotation;
        public override MobTransition Transition => MobTransition.RoamRotation;

        public RoamRotationMobTransition(IMobPersistentData persistentData, IRoamArea roamArea, IMobPerUpdateData perUpdateData)
        {
            _persistentData = persistentData;
            _roamArea = roamArea;
            _perUpdateData = perUpdateData;
        }

        protected override void FillConditionForStates()
        {
            base.FillConditionForStates();

            ConditionForState[MobState.Roam] = () => _persistentData.IsRoamMoveOnPathFinished;
            ConditionForState[MobState.ReturnToRoamArea] = () => _roamArea.IsInside(_perUpdateData.Position);
        }
    }
}