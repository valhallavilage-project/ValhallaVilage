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
        private IDamageReceiveHandler _damageReceiveHandler;

        [Inject]
        public void AddDependencies(IDamageReceiveHandler damageReceiveHandler)
        {
            _damageReceiveHandler = damageReceiveHandler;
        }

        public void ReceiveDamage(float damage)
        {
            _damageReceiveHandler.ReceiveDamage(damage);
        }
    }
}
