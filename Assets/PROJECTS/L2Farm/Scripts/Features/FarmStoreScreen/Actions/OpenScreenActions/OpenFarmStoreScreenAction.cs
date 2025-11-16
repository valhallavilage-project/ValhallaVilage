using CrossProject.Core.Actions;
using Cysharp.Threading.Tasks;

namespace L2Farm
{
    public class OpenFarmStoreScreenAction : Action<OpenFarmStoreScreenActionConfig>
    {
        private readonly IGlobalOpenFarmStoreScreenHandler _openFarmStoreScreenHandler;

        public OpenFarmStoreScreenAction(IGlobalOpenFarmStoreScreenHandler openFarmStoreScreenHandler)
        {
            _openFarmStoreScreenHandler = openFarmStoreScreenHandler;
        }
        
        public override async UniTask Execute()
        {
            _openFarmStoreScreenHandler.OpenScreen();
        }
    }
}
