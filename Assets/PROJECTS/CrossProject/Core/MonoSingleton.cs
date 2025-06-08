using UnityEngine;

namespace CrossProject.Core
{
    public class MonoSingleton<T> : CustomBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);

            if (this is T instance)
                Instance = instance;

            OnAwake();
        }
    }
}