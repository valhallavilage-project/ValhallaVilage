using CrossProject.Core.Actions;
using Cysharp.Threading.Tasks;

namespace L2Farm
{
    public class ActivateGardenAction : Action<ActivateGardenActionConfig>
    {
        private readonly IGlobalActivateGardenHandler _globalActivateGardenHandler;

        public ActivateGardenAction(IGlobalActivateGardenHandler globalActivateGardenHandler)
        {
            _globalActivateGardenHandler = globalActivateGardenHandler;
        }

        public override async UniTask Execute()
        {
            _globalActivateGardenHandler.ActivateGardenBeds();
        }
    }
}
