using System;
using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IHealthHandler : ILifetimeParameterHandler
    {
        IReadOnlyAsyncReactiveProperty<float> MaxHealth { get; }
        IReadOnlyAsyncReactiveProperty<float> Health { get; }
        bool IsDied { get; }

        void Init(float maxHealth, float currentHealth);
        void Damage(float value);
        void IncreaseMaxHealth(float value);
        void ReduceMaxHealth(float value);
        void AssignMaxHealth(float value);
    }

    public class HealthHandler : BoxedFloatValue, IHealthHandler
    {
        public IReadOnlyAsyncReactiveProperty<float> MaxHealth => _maxValue;
        public IReadOnlyAsyncReactiveProperty<float> Health => _currentValue;

        public void Init(float maxHealth, float currentHealth)
        {
            base.Init(maxHealth, currentHealth, 0);
        }

        public bool IsFullyRestored => Math.Abs(Health.Value - MaxHealth.Value) < float.Epsilon;
        public bool IsDied => Health.Value <= _minValue.Value;

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
            IncreaseCurrentValue(value);
        }

        public void ReduceMaxHealth(float value)
        {
            ReduceMaxValue(value);
            ReduceCurrentValue(value);
        }

        public void AssignMaxHealth(float value)
        {
            var ratio = _currentValue.Value / _maxValue.Value;
            
            AssignMaxValue(value);
            
            var newValue = ratio * _maxValue.Value;
            var delta = newValue  - _currentValue.Value;

            if (delta > 0)
            {
                Restore(Math.Abs(delta));
            }
            else
            {
                Damage(Math.Abs(delta));
            }
        }
    }
}
