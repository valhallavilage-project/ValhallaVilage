using System;
using System.Collections.Generic;
using CrossProject.Core.SaveLoad;
using UnityEngine;

namespace L2Farm
{
    [Serializable]
    public class GardenBedState
    {
        public GardenBedStateType State;
        public long StartTimeTicks;
        public long FinishTimeTicks;
        public string ResourceId;
        public int Amount;
    }

    [Serializable]
    public class GardenStatePart : IGameStatePart
    {
        public Dictionary<string, GardenBedStateType> GardenBedStates { get; set; } = new Dictionary<string, GardenBedStateType>();
        public Dictionary<string, GardenBedState> DetailedStates { get; set; } = new Dictionary<string, GardenBedState>();

        public GardenBedStateType GetGardenBedState(string id)
        {
            if (DetailedStates.TryGetValue(id, out var detailedState))
            {
                return detailedState.State;
            }
            return !GardenBedStates.TryGetValue(id, out var state) ? GardenBedStateType.Deactivated : state;
        }

        public void UpdateGardenBedState(string id, GardenBedStateType stateType)
        {
            GardenBedStates[id] = stateType;
            
            if (!DetailedStates.ContainsKey(id))
            {
                DetailedStates[id] = new GardenBedState { State = stateType };
            }
            else
            {
                DetailedStates[id].State = stateType;
            }
        }

        public GardenBedState GetDetailedState(string id)
        {
            if (!DetailedStates.TryGetValue(id, out var state))
            {
                state = new GardenBedState { State = GetGardenBedState(id) };
                DetailedStates[id] = state;
            }
            return state;
        }
    }
}
