using System;
using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IHealthHandler
    {
        IReadOnlyAsyncReactiveProperty<float> MaxHealth { get; }
        IReadOnlyAsyncReactiveProperty<float> Health { get; }

        void Init(float maxHealth, float currentHealth);
        void Damage(float value);
        void Restore(float value);
        void Increase(float value);
        void Decrease(float value);
    }

    public class HealthHandler : IHealthHandler
    {
        private readonly AsyncReactiveProperty<float> _maxHealth = new(default);
        private readonly AsyncReactiveProperty<float> _health = new(default);

        public IReadOnlyAsyncReactiveProperty<float> MaxHealth => _maxHealth;
        public IReadOnlyAsyncReactiveProperty<float> Health => _health;

        public void Init(float maxHealth, float currentHealth)
        {
            _maxHealth.Value = maxHealth;
            _health.Value = currentHealth;
        }

        public void Damage(float value)
        {
            var health = _health.Value;

            health -= value;

            _health.Value = Math.Max(health, 0);
        }

        public void Restore(float value)
        {
            var health = _health.Value;

            health += value;

            _health.Value = Math.Min(health, _maxHealth.Value);
        }

        public void Increase(float value)
        {
            _maxHealth.Value += value;
            _health.Value += value;
        }

        public void Decrease(float value)
        {
            _maxHealth.Value -= value;
            _health.Value = Math.Min(_health.Value, _maxHealth.Value);
        }
    }
}
