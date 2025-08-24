using System.Collections.Generic;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core.InGameResources
{
    public class ResourceContentPart : IGameStatePart
    {
        public Dictionary<ResourceId, int> Resources { get; set; } = new ();
    }
}