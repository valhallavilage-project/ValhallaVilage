using CrossProject.Core.Actions;
using Cysharp.Threading.Tasks;

namespace L2Farm.Features.Buildings.Actions
{
    public class FinishBuildingAction : Action<FinishBuildingActionConfig>
    {
        private readonly BuildingService _buildingService;

        public FinishBuildingAction(BuildingService buildingService)
        {
            _buildingService = buildingService;
        }

        public override async UniTask Execute()
        {
            await _buildingService.SpawnReadyBuilding(config.buildingId);
        }
    }
}
