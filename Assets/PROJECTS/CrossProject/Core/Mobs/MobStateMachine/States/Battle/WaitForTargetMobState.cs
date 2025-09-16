using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public class WaitForTargetMobState : BaseMobState
    {
        private readonly IAgroArea _agroArea;
        private readonly IMobPerUpdateData _perUpdateData;
        private readonly IRotateAbility _rotateAbility;
        
        public override MobState State => MobState.WaitForTarget;

        public WaitForTargetMobState(IRotateAbility rotateAbility, IAgroArea agroArea, IMobPerUpdateData perUpdateData)
        {
            _rotateAbility = rotateAbility;
            _agroArea = agroArea;
            _perUpdateData = perUpdateData;
        }

        protected override async UniTask HandleControl()
        {
            await base.HandleControl();

            var direction = Vector3.ProjectOnPlane(_agroArea.Enemy.position - _perUpdateData.Position, Vector3.up);

            _rotateAbility.ForceRotate(direction.normalized);
        }
    }
}
