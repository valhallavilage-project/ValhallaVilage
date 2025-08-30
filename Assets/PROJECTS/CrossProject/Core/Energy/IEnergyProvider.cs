using System;

namespace CrossProject.Core.Energy
{
    public interface IEnergyProvider
    {
        int CurrentValue { get; }
        int MaxValue { get; }
        void Spend(int amount);
        event Action<int, int> OnEnergyChanged; //old and current value
    }
}
