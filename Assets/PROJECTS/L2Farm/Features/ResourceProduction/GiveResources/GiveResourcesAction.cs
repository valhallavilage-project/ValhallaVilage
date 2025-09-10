using CrossProject.Core.Actions;
using CrossProject.Core.InGameResources;
using CrossProject.Core.SaveLoad;

namespace L2Farm.Features.ResourceProduction.GiveResources
{
    public class GiveResourcesAction : Action<GiveResourceActionConfig>
    {
        private readonly GameStateManager _gameStateManager;

        public override void Execute()
        {
            var part = _gameStateManager.State.Get<ResourceContentPart>();
            if (part.Resources.ContainsKey(config.resourceId))
                part.Resources[config.resourceId] += config.amount;
            else
                part.Resources[config.resourceId] = config.amount;
        }
    }
}
