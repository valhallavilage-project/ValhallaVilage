using CrossProject.Core.Actions;

namespace L2Farm.Features.Buildings.Actions
{
    public class UpgradeBuildingAction : Action<UpgradeBuildingActionConfig>
    {
        private readonly BuildingService _buildingService;

        public UpgradeBuildingAction(BuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        public override void Execute()
        {
            _buildingService.SpawnReadyBuilding(config.buildingId);
        }
    }
}
