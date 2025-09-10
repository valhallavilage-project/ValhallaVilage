using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public class ApproachTargetMobState : BaseMobState
    {
        private readonly IMoveAbility _moveAbility;
        private readonly IRotateAbility _rotateAbility;
        private readonly INoticeEnemyArea _noticeEnemyArea;
        private readonly IMobPerUpdateData _perUpdateData;
        private readonly IMobPersistentData _persistentData;

        public override MobState State => MobState.ApproachTarget;

        public ApproachTargetMobState(IMoveAbility moveAbility, IRotateAbility rotateAbility, INoticeEnemyArea noticeEnemyArea,
            IMobPerUpdateData perUpdateData, IMobPersistentData persistentData)
        {
            _moveAbility = moveAbility;
            _rotateAbility = rotateAbility;
            _noticeEnemyArea = noticeEnemyArea;
            _perUpdateData = perUpdateData;
            _persistentData = persistentData;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            _persistentData.IsTargetApproached = false;
        }

        protected override async UniTask HandleControl()
        {
            await base.HandleControl();

            var distance = _noticeEnemyArea.Enemy.position - _perUpdateData.Position;

            if (distance.magnitude <= Config.AttackDistance)
            {
                _persistentData.IsTargetApproached = true;
                return;
            }

            var direction = distance.normalized;

            _moveAbility.Move(direction, Config.MaxSpeed);
            _rotateAbility.ForceRotate(direction);
        }

        public override async UniTask Exit()
        {
            await base.Exit();

            _persistentData.IsTargetApproached = false;
        }
    }
}