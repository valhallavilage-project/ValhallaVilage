using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IEnergyHandler
    {
        IReadOnlyAsyncReactiveProperty<float> MaxEnergy { get; }
        IReadOnlyAsyncReactiveProperty<float> Energy { get; }

        void Init(float maxHealth, float currentHealth);
        void Restore(float value);
        void Spend(float value);
        void IncreaseMaxEnergy(float value);
        void ReduceMaxEnergy(float value);
    }

    public class EnergyHandler : BoxedFloatValue, IEnergyHandler
    {
        public IReadOnlyAsyncReactiveProperty<float> MaxEnergy => _maxValue;
        public IReadOnlyAsyncReactiveProperty<float> Energy => _currentValue;

        public void Init(float maxHealth, float currentHealth)
        {
            base.Init(maxHealth, currentHealth, 0);
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
