using System;
using System.Collections.Generic;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core.InGameResources
{
    public class ResourceContentPart : IGameStatePart
    {
        public event Action<ResourceId, int> OnResourceChange;

        public Dictionary<ResourceId, int> Resources { get; set; } = new ();

        public void Edit(ResourceId id, int amount)
        {
            if (Resources.ContainsKey(id))
                Resources[id] += amount;
            else
                Resources[id] = amount;
            OnResourceChange?.Invoke(id, amount);
        }
    }
}