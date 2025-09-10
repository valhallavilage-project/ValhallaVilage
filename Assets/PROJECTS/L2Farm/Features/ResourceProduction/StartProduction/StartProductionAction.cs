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

        public override void Execute()
        {
            _productionService.StartProduction(config.productionId).Forget();
        }
    }
}
