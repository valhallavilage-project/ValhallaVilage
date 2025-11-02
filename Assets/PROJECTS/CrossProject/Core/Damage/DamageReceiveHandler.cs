using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IDamageReceiveHandler
    {
        IReadOnlyAsyncReactiveProperty<float> DamageReceived { get; }
        
        void ReceiveDamage(float value);
    }

    public class DamageReceiveHandler : IDamageReceiveHandler
    {
        private readonly IHealthHandler _healthHandler;

        private readonly AsyncReactiveProperty<float> _damageReceived = new(default);

        public IReadOnlyAsyncReactiveProperty<float> DamageReceived => _damageReceived;

        public DamageReceiveHandler(IHealthHandler healthHandler)
        {
            _healthHandler = healthHandler;
        }

        public void ReceiveDamage(float value)
        {
            _healthHandler.Damage(value);

            _damageReceived.Value = value;
        }
    }
}
