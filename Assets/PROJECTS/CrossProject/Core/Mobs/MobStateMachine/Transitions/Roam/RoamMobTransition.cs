namespace CrossProject.Core
{
    public class RoamMobTransition : BaseMobTransition
    {
        private readonly IRoamArea _roamArea;
        private readonly IMobPersistentData _persistentData;
        private readonly IMobPerUpdateData _perUpdateData;

        public override MobState State => MobState.Roam;
        public override MobTransition Transition => MobTransition.Roam;

        public RoamMobTransition(IRoamArea roamArea, IMobPersistentData persistentData, IMobPerUpdateData perUpdateData)
        {
            _roamArea = roamArea;
            _persistentData = persistentData;
            _perUpdateData = perUpdateData;
        }

        protected override void FillConditionForStates()
        {
            base.FillConditionForStates();

            ConditionForState[MobState.RoamRotation] = () => _persistentData.IsRoamRotationFinished && _roamArea.IsInside(_perUpdateData.Position);
        }
    }
}