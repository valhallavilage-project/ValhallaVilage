using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class ManualPrefabInjector : MonoBehaviour
    {
        public static ManualPrefabInjector Instance;

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
            _objectResolver.Inject(component);
        }
    }
}
