using Unity.AI.Navigation;
using UnityEngine;

namespace CrossProject.Core
{
    [DefaultExecutionOrder(-100)]
    public class NavMeshLoader : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface _surface;

        private void Awake()
        {
            if (_surface == null)
                _surface = GetComponent<NavMeshSurface>();

            if (_surface == null)
            {
                Debug.LogError($"[NavMeshLoader] No NavMeshSurface found on {gameObject.name}");
                return;
            }

            if (_surface.navMeshData != null)
            {
                _surface.RemoveData();
                _surface.AddData();
                Debug.Log($"[NavMeshLoader] NavMesh data loaded: {gameObject.name}");
            }
            else
            {
                Debug.LogWarning($"[NavMeshLoader] No NavMesh data on: {gameObject.name}");
            }
        }

        private void OnDestroy()
        {
            if (_surface != null && _surface.navMeshData != null)
            {
                _surface.RemoveData();
            }
        }
    }
}
