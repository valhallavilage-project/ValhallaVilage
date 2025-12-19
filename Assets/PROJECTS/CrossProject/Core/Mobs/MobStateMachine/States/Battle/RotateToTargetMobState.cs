using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public class RotateToTargetMobState : BaseMobState
    {
        private readonly IRotateAbility _rotateAbility;
        private readonly IAgroArea _agroArea;
        private readonly IMobPerUpdateData _perUpdateData;
        private readonly IMobPersistentData _persistentData;
        private readonly IMoveAbility _moveAbility;

        public override MobState State => MobState.RotateToTarget;

        public RotateToTargetMobState(IRotateAbility rotateAbility, IAgroArea agroArea,
            IMobPerUpdateData perUpdateData, IMobPersistentData persistentData, IMoveAbility moveAbility)
        {
            _rotateAbility = rotateAbility;
            _agroArea = agroArea;
            _perUpdateData = perUpdateData;
            _persistentData = persistentData;
            _moveAbility = moveAbility;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            _moveAbility.StopMovement();

            _persistentData.IsRotationToTargetFinished = false;
        }

        protected override async UniTask HandleControl()
        {
            await base.HandleControl();

            // Fix: Check if Enemy exists before accessing position
            if (_agroArea.Enemy == null)
            {
                Debug.LogWarning("[RotateToTargetMobState] Enemy is null, cannot rotate to target");
                _persistentData.IsRotationToTargetFinished = true;
                return;
            }

            const float differenceAngles = 1;
            const float directionsMinDifference = 1 - differenceAngles * Mathf.Deg2Rad;

            var direction = Vector3.ProjectOnPlane(_agroArea.Enemy.position - _perUpdateData.Position, Vector3.up);

            var difference = MathUtils.GetVectorsDirectionDifference(direction.normalized, _perUpdateData.Rotation * Vector3.forward, Vector3.up);

            if (difference > directionsMinDifference)
            {
                _persistentData.IsRotationToTargetFinished = true;
                _rotateAbility.ForceRotate(direction.normalized);
            }

            _rotateAbility.Rotate(direction.normalized);
        }

        public override async UniTask Exit()
        {
            await base.Exit();

            _persistentData.IsRotationToTargetFinished = false;
        }
    }
}