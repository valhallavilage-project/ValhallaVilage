using System;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core
{
    [Serializable]
    public class EnergyStatePart : IGameStatePart
    {
        public float Value { get; set; }
        public DateTime LastRestoreTime { get; set; }
    }
}
