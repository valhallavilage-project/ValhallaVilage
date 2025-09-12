using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public interface IDamageReceiver
    {
        void ReceiveDamage(float damage);
    }

    public class DamageReceiver : MonoBehaviour, IDamageReceiver
    {
        private IHealthHandler _healthHandler;

        [Inject]
        public void AddDependencies(IHealthHandler hitHandler)
        {
            _healthHandler = hitHandler;
        }

        public void ReceiveDamage(float damage)
        {
            _healthHandler.Decrease(damage);
        }
    }
}
