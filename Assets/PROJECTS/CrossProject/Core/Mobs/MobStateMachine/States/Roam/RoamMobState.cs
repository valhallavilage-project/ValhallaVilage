using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public class RoamMobState : BaseMobState
    {
        private readonly IMoveAbility _moveAbility;
        private readonly IMobPerUpdateData _perUpdateData;
        private readonly IMobPersistentData _persistentData;
        private readonly IRotateAbility _rotateAbility;

        private float _stateEnterTime;

        public override MobState State => MobState.Roam;

        public RoamMobState(IMoveAbility moveAbility, IMobPerUpdateData perUpdateData, IMobPersistentData persistentData,
            IRotateAbility rotateAbility)
        {
            _moveAbility = moveAbility;
            _perUpdateData = perUpdateData;
            _persistentData = persistentData;
            _rotateAbility = rotateAbility;
        }

        public override async UniTask Enter()
        {
            await base.Enter();
            _stateEnterTime = Time.time;
        }

        protected override async UniTask HandleControl()
        {
            await base.HandleControl();

            // Minimum time in state to prevent jittering when mobs collide
            var timeInState = Time.time - _stateEnterTime;
            if (timeInState < Config.RoamingMinTimeInState)
            {
                _moveAbility.Move(_persistentData.RoamPathDirection, Config.RoamingMaxSpeed);
                _rotateAbility.ForceRotate(_persistentData.RoamPathDirection);
                return;
            }

            if (_perUpdateData.Position.IsDestinationReached(_persistentData.RoamPathDestination, _persistentData.RoamPathStart, 0.1f, true))
            {
                _persistentData.IsRoamMoveOnPathFinished = true;
                return;
            }

            _moveAbility.Move(_persistentData.RoamPathDirection, Config.RoamingMaxSpeed);
            _rotateAbility.ForceRotate(_persistentData.RoamPathDirection);

            var destination = new Vector3(_persistentData.RoamPathDestination.x, _perUpdateData.Position.y, _persistentData.RoamPathDestination.z);

            Debug.DrawLine(_perUpdateData.Position, destination, Color.red);
        }

        public override async UniTask Exit()
        {
            await base.Enter();

            _persistentData.IsRoamMoveOnPathFinished = false;
        }
    }
}