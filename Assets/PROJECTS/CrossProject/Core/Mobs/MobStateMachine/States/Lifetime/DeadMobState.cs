using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public class DeadMobState : BaseMobState
    {
        private readonly IDieAbility _dieAbility;
        public override MobState State => MobState.Dead;

        public DeadMobState(IDieAbility dieAbility)
        {
            _dieAbility = dieAbility;
        }

        public override async UniTask Enter()
        {
            await base.Enter();
            
            _dieAbility.DeadCompletely();
        }
    }
}
