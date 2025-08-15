using System;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core.Energy
{
    [Serializable]
    public class EnergyStatePart : IGameStatePart
    {
        public int energyValue = -1;
        public DateTime lastEnergySpent;
    }
}
