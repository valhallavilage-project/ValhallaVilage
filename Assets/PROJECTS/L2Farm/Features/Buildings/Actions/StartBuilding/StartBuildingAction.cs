using CrossProject.Core.Actions;
using Cysharp.Threading.Tasks;

namespace L2Farm.Features.Buildings.Actions
{
    public class StartBuildingAction : Action<StartBuildingActionConfig>
    {
        private readonly BuildingService _buildingService;

        public StartBuildingAction(BuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        public override void Execute()
        {
            _buildingService.StartUpgradeProcess(config.buildingId).Forget();
        }
    }
}
