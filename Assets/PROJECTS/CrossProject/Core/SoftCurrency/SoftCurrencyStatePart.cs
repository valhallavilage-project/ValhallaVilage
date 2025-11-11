using System;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core
{
    [Serializable]
    public class SoftCurrencyStatePart : IGameStatePart
    {
        public int Amount { get; set; }
    }
}
