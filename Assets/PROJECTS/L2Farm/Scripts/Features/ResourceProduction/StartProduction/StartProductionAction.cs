using CrossProject.Core.Actions;
using Cysharp.Threading.Tasks;

namespace L2Farm.Features.ResourceProduction.Actions
{
    public class StartProductionAction : Action<StartProductionActionConfig>
    {
        private readonly ProductionService _productionService;

        public StartProductionAction(ProductionService productionService)
        {
            _productionService = productionService;
        }

        public override async UniTask Execute()
        {
            await _productionService.StartProduction(config.productionId);
        }
    }
}
