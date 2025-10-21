using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public class ReturnToRoamAreaMobState : BaseMobState
    {
        private readonly IMoveAbility _moveAbility;
        private readonly IMobPerUpdateData _perUpdateData;
        private readonly IMobPersistentData _persistentData;
        private readonly IRotateAbility _rotateAbility;
        private readonly IAgroArea _agroArea;

        public override MobState State => MobState.ReturnToRoamArea;
        
        public ReturnToRoamAreaMobState(IMoveAbility moveAbility, IMobPerUpdateData perUpdateData, IMobPersistentData persistentData,
            IRotateAbility rotateAbility, IAgroArea agroArea)
        {
            _moveAbility = moveAbility;
            _perUpdateData = perUpdateData;
            _persistentData = persistentData;
            _rotateAbility = rotateAbility;
            _agroArea = agroArea;
        }

        public override async UniTask Enter()
        {
            await base.Enter();
            
            _agroArea.ForgetEnemy();
        }

        protected override async UniTask HandleControl()
        {
            await base.HandleControl();
            
            var destination = new Vector3(_persistentData.RoamPathDestination.x, _perUpdateData.Position.y, _persistentData.RoamPathDestination.z);

            var direction = (destination - _perUpdateData.Position).normalized;
            
            _moveAbility.Move(direction, Config.RoamingMaxSpeed);
            _rotateAbility.ForceRotate(direction);

            Debug.DrawLine(_perUpdateData.Position, destination, Color.red);
        }
    }
}