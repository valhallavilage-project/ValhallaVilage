using System.Collections.Generic;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core.InGameResources
{
    public class ResourceContentPart : IGameStatePart
    {
        public List<ResourceContent> Resources { get; set; } = new();
    }
}