using System;
using System.Collections.Generic;
using CrossProject.Core.SaveLoad;

namespace L2Farm
{
    [Serializable]
    public class GardenStatePart : IGameStatePart
    {
        public Dictionary<string, GardenBedStateType> GardenBedStates { get; set; } = new Dictionary<string, GardenBedStateType>();

        public GardenBedStateType GetGardenBedState(string id)
        {
            return !GardenBedStates.TryGetValue(id, out var state) ? GardenBedStateType.Deactivated : state;
        }

        public void UpdateGardenBedState(string id, GardenBedStateType stateType)
        {
            GardenBedStates[id] = stateType;
        }
    }
}
