using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using L2Farm.Features.ResourceProduction;

namespace L2Farm
{
    public class ProductionTimerCallback : ITimerCallback
    {
        private readonly GameStateManager _gameStateManager;
        private readonly ProductionId _productionId;

        public ProductionTimerCallback(GameStateManager gameStateManager, ProductionId productionId)
        {
            _gameStateManager = gameStateManager;
            _productionId = productionId;
        }

        public async UniTask Execute()
        {
            _gameStateManager.State.Get<ProductionPart>().requests.Remove(_productionId);
        }
    }
}