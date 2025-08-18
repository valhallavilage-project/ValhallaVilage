using System.Collections.Generic;
using System.Linq;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core.InGameResources
{
    public class ResourceContentPart : IGameStatePart
    {
        public List<ResourceContent> Resources { get; set; } = new();

        public int Has(ResourceId id)
        {
            var content = Resources.FirstOrDefault(x => x.Resource == id);
            return content?.Amount ?? 0;
        }
    }
}