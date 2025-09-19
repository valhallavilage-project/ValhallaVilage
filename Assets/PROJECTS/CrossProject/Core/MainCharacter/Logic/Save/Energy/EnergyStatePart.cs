using System;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core
{
    [Serializable]
    public class EnergyStatePart : IGameStatePart, IRestorableParameterStatePart
    {
        public float Value { get; set; }
        public DateTime LastRestoreTime { get; set; }
    }
}
