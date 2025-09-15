using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public class AttackPauseMobState : BaseMobState
    {
        private readonly IRotateAbility _rotateAbility;
        private readonly IAgroArea _agroArea;
        private readonly IMobPerUpdateData _perUpdateData;

        public override MobState State => MobState.AttackPause;

        public AttackPauseMobState(IRotateAbility rotateAbility, IAgroArea agroArea, IMobPerUpdateData perUpdateData)
        {
            _rotateAbility = rotateAbility;
            _agroArea = agroArea;
            _perUpdateData = perUpdateData;
        }

        protected override async UniTask HandleControl()
        {
            await base.HandleControl();

            var distance = Vector3.ProjectOnPlane(_agroArea.Enemy.position - _perUpdateData.Position, Vector3.up);

            _rotateAbility.ForceRotate(distance.normalized);
        }
    }
}