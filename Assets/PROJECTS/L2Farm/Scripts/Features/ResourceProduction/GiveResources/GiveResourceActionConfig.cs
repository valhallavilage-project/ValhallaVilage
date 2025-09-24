using CrossProject.Core.Actions;
using CrossProject.Core.InGameResources;

namespace L2Farm.Features.ResourceProduction.GiveResources
{
    [System.Serializable]
    public class GiveResourceActionConfig : IActionConfig
    {
        public ResourceId resourceId;
        public int amount;
    }
}
