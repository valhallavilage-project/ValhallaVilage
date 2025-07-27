using System;

namespace CrossProject.Core.Energy
{
    public interface IEnergyProvider
    {
        int CurrentValue { get; }
        int MaxValue { get; }
        void Spend(int amount);
        event Action<int, int> OnEnergySpend; //spend amount and current value
    }
}
