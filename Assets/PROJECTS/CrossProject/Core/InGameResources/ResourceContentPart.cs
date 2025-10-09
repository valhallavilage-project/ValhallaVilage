using System.Collections.Generic;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core.InGameResources
{
    public class ResourceContentPart : IGameStatePart
    {
        public Dictionary<ResourceId, int> Resources { get; set; } = new();

        public void Edit(ResourceId id, int amount)
        {
            if (Resources.ContainsKey(id))
            {
                Resources[id] += amount;
            }
            else
            {
                Resources[id] = amount;
            }
        }

        public int Get(ResourceId id)
        {
            return Resources.ContainsKey(id) ? Resources[id] : 0;
        }
    }
}
