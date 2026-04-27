using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class Injector : MonoBehaviour
    {
        public static Injector Instance;

        private IObjectResolver _objectResolver;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        [Inject]
        private void Construct(IObjectResolver objectResolver)
        {
            _objectResolver = objectResolver;
        }

        public void Inject<TComponent>(TComponent component) where TComponent : MonoBehaviour
        {
            if (_objectResolver == null)
            {
                Debug.LogWarning($"[Injector] Inject called before DI was constructed. Target: {(component != null ? component.gameObject.name : "null")}");
                return;
            }
            _objectResolver.Inject(component);
        }
    }
}
