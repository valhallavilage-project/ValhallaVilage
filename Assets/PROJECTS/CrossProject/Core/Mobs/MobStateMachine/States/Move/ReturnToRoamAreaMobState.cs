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
        
        public override MobState State => MobState.ReturnToRoamArea;
        
        public ReturnToRoamAreaMobState(IMoveAbility moveAbility, IMobPerUpdateData perUpdateData, IMobPersistentData persistentData,
            IRotateAbility rotateAbility)
        {
            _moveAbility = moveAbility;
            _perUpdateData = perUpdateData;
            _persistentData = persistentData;
            _rotateAbility = rotateAbility;
        }
        
        protected override async UniTask HandleControl()
        {
            await base.HandleControl();

            _moveAbility.Move(_persistentData.RoamPathDirection, Config.RoamingMaxSpeed);
            _rotateAbility.ForceRotate(_persistentData.RoamPathDirection);

            var destination = new Vector3(_persistentData.RoamPathDestination.x, _perUpdateData.Position.y, _persistentData.RoamPathDestination.z);

            Debug.DrawLine(_perUpdateData.Position, destination, Color.red);
        }
    }
}