using CrossProject.Core.Actions;

namespace L2Farm.Features.Buildings.Actions
{
    public class FinishBuildingAction : Action<FinishBuildingActionConfig>
    {
        private readonly BuildingService _buildingService;

        public FinishBuildingAction(BuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        public override void Execute()
        {
            _buildingService.SpawnReadyBuilding(config.buildingId);
        }
    }
}
