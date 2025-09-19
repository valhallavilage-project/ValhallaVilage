using System;
using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IEnergyHandler : ILifetimeParameterHandler
    {
        IReadOnlyAsyncReactiveProperty<float> MaxEnergy { get; }
        IReadOnlyAsyncReactiveProperty<float> Energy { get; }
        bool IsFullyRestored { get; }

        void Init(float maxEnergy, float currentEnergy);
        void Spend(float value);
        void IncreaseMaxEnergy(float value);
        void ReduceMaxEnergy(float value);
    }

    public class EnergyHandler : BoxedFloatValue, IEnergyHandler
    {
        public IReadOnlyAsyncReactiveProperty<float> MaxEnergy => _maxValue;
        public IReadOnlyAsyncReactiveProperty<float> Energy => _currentValue;

        public bool IsFullyRestored => Math.Abs(Energy.Value - MaxEnergy.Value) < float.Epsilon;

        public void Init(float maxEnergy, float currentEnergy)
        {
            base.Init(maxEnergy, currentEnergy, 0);
        }

        public void Restore(float value)
        {
            IncreaseCurrentValue(value);
        }

        public void Spend(float value)
        {
            ReduceCurrentValue(value);
        }

        public void IncreaseMaxEnergy(float value)
        {
            IncreaseMaxValue(value);
        }

        public void ReduceMaxEnergy(float value)
        {
            ReduceMaxValue(value);
        }
    }
}
