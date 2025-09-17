using System;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core
{
    [Serializable]
    public class HealthStatePart : IGameStatePart
    {
        public float Value { get; set; }
    }
}
