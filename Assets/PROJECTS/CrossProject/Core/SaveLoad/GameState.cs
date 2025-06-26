using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CrossProject.Core.SaveLoad
{
    public class GameState
    {
        // public string ClientVersion { get; set; }
        // public string DeviceId { get; set; }
        // public DateTime CreatedAt { get; set; }

        [JsonProperty]
        private Dictionary<Type, IGameStatePart> _partsMap = new();

        public TGameStatePart Get<TGameStatePart>() where TGameStatePart : class, IGameStatePart
        {
            return null;
        }

        public void Set<TGameStatePart>(TGameStatePart part) where TGameStatePart : class, IGameStatePart
        {
            
        }
    }
}