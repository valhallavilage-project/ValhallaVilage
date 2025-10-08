using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using L2Farm.Features.Buildings;

namespace L2Farm
{
    public class BuildingTimerCallback : ITimerCallback
    {
        private readonly GameStateManager _gameStateManager;
        private readonly BuildingId _buildingId;

        public BuildingTimerCallback(GameStateManager gameStateManager, BuildingId buildingId)
        {
            _gameStateManager = gameStateManager;
            _buildingId = buildingId;
        }

        public async UniTask Execute()
        {
            _gameStateManager.State.Get<BuildingPart>().requests.Remove(_buildingId);
        }
    }
}