using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CrossProject.Core.SaveLoad
{
    public class GameState
    {
        [JsonProperty]
        private Dictionary<Type, IGameStatePart> _partsMap = new();

        public bool TryGet<TGameStatePart>(out TGameStatePart part) where TGameStatePart : class, IGameStatePart
        {
            var type = typeof(TGameStatePart);
            if (_partsMap.ContainsKey(type))
            {
                part = _partsMap[type] as TGameStatePart;
                return true;
            }

            part = null;
            return false;
        }

        public TGameStatePart Set<TGameStatePart>(TGameStatePart part) where TGameStatePart : class, IGameStatePart
        {
            _partsMap[typeof(TGameStatePart)] = part;
            return part;
        }
    }
}