using CrossProject.Core.Actions;
using CrossProject.Core.InGameResources;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;

namespace L2Farm.Scripts.Actions.SpendResources
{
    public class SpendResourcesAction : Action<SpendResourcesActionConfig>
    {
        private readonly GameStateManager _gameStateManager;

        public SpendResourcesAction(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }

        public override async UniTask Execute()
        {
            var part = _gameStateManager.State.Get<ResourceContentPart>();
            foreach (var resourceCondition in config.resourceConditions)
            {
                part.Resources[resourceCondition.resourceId] -= resourceCondition.neededQuantity;
            }
            _gameStateManager.Save();
        }
    }
}
