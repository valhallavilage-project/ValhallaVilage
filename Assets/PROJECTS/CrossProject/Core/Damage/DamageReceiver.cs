using UnityEngine;

namespace CrossProject.Core
{
    public interface IDamageReceiver
    {
        void ReceiveDamage(float damage);
    }

    public class DamageReceiver : MonoBehaviour, IDamageReceiver
    {
        public void ReceiveDamage(float damage)
        {
            Debug.Log($"Received damage {damage}");
        }
    }
}
