using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public class DieMobState : BaseMobState
    {
        private readonly IMoveAbility _moveAbility;
        private readonly IRotateAbility _rotateAbility;
        private readonly IDieAbility _dieAbility;

        public override MobState State => MobState.Die;

        public DieMobState(IMoveAbility moveAbility, IRotateAbility rotateAbility, IDieAbility dieAbility)
        {
            _moveAbility = moveAbility;
            _rotateAbility = rotateAbility;
            _dieAbility = dieAbility;
        }

        public override async UniTask Enter()
        {
            await base.Enter();

            _moveAbility.StopMovement();
            _rotateAbility.StopRotation();
            
            _dieAbility.BeginToDie();
        }
    }
}
