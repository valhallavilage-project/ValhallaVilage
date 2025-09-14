using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IHealthHandler : IBoxedValueHandler<float>
    {
        IReadOnlyAsyncReactiveProperty<float> MaxHealth { get; }
        IReadOnlyAsyncReactiveProperty<float> Health { get; }

        void Init(float maxHealth, float currentHealth);
        void Damage(float value);
        void Restore(float value);
        void IncreaseMaxHealth(float value);
        void ReduceMaxHealth(float value);
    }

    public class HealthHandler : BoxedFloatValue, IHealthHandler
    {
        public IReadOnlyAsyncReactiveProperty<float> MaxHealth => _maxValue;
        public IReadOnlyAsyncReactiveProperty<float> Health => _currentValue;

        public void Init(float maxHealth, float currentHealth)
        {
            base.Init(maxHealth, currentHealth, 0);
        }

        public void Restore(float value)
        {
            IncreaseCurrentValue(value);
        }

        public void Damage(float value)
        {
            ReduceCurrentValue(value);
        }

        public void IncreaseMaxHealth(float value)
        {
            IncreaseMaxValue(value);
        }

        public void ReduceMaxHealth(float value)
        {
            ReduceMaxValue(value);
        }
    }
}
