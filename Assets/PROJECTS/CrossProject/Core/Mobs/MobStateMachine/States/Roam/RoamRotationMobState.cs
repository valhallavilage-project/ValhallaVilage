using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public class RoamRotationMobState : BaseMobState
    {
        private readonly IRoamArea _roamArea;
        private readonly IRotateAbility _rotateAbility;
        private readonly IMobPerUpdateData _perUpdateData;
        private readonly IMobPersistentData _persistentData;
        private readonly IMoveAbility _moveAbility;
        private readonly MobConfig _mobConfig;

        public override MobState State => MobState.RoamRotation;

        public RoamRotationMobState(IRoamArea roamArea, IRotateAbility rotateAbility, IMobPerUpdateData perUpdateData,
            IMobPersistentData persistentData, IMoveAbility moveAbility, MobConfig mobConfig)
        {
            _roamArea = roamArea;
            _rotateAbility = rotateAbility;
            _perUpdateData = perUpdateData;
            _persistentData = persistentData;
            _moveAbility = moveAbility;
            _mobConfig = mobConfig;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            _rotateAbility.StopRotation();
            _moveAbility.StopMovement();
            _persistentData.IsRoamRotationFinished = false;

            const int pathGenerationTries = 10;

            for (var i = 0; i < pathGenerationTries; i++)
            {
                _persistentData.RoamPathDestination = _roamArea.GetPointInside();
                _persistentData.RoamPathStart = Vector3.ProjectOnPlane(_perUpdateData.Position, Vector3.up);

                var path = _persistentData.RoamPathDestination - _persistentData.RoamPathStart;

                _persistentData.RoamPathDirection = path.normalized;

                if (Vector3.ProjectOnPlane(path, Vector3.up).magnitude > _roamArea.MinRoamPathLength)
                {
                    break;
                }
            }
        }

        protected override async UniTask HandleControl()
        {
            await base.HandleControl();

            var directionsMinDifference = 1 - _mobConfig.RoamingMinAngleBeforeForceRotate * Mathf.Deg2Rad;

            var difference = MathUtils.GetVectorsDirectionDifference(_persistentData.RoamPathDirection, _perUpdateData.Rotation * Vector3.forward, Vector3.up);

            if (difference > directionsMinDifference)
            {
                _persistentData.IsRoamRotationFinished = true;
                _rotateAbility.ForceRotate(_persistentData.RoamPathDirection);
                return;
            }

            _rotateAbility.Rotate(_persistentData.RoamPathDirection);
        }

        public override async UniTask Exit()
        {
            await base.Exit();

            _rotateAbility.StopRotation();
        }
    }
}