using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public interface IMainCharacterReviveGlobalHandler
    {
        IReadOnlyAsyncReactiveProperty<Invoker> Revived { get; }
        Transform RevivePoint { get; }

        void Revive();
        void InitRevivePoint(Transform transform);
    }

    public class MainCharacterGlobalReviveHandler : IMainCharacterReviveGlobalHandler
    {
        private readonly AsyncReactiveProperty<Invoker> _revived = new(default);
        private Transform _revivePoint;

        public Transform RevivePoint
        {
            get
            {
                if (_revivePoint == null)
                {
                    var allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);
                    foreach (var go in allObjects)
                    {
                        if (go.name == "RevivePoint" || go.name == "Respawn")
                        {
                            // Проверяем, что это не часть префаба игрока
                            if (go.GetComponentInParent<SimpleMovement.SimpleMovementController>() == null)
                            {
                                _revivePoint = go.transform;
                                return _revivePoint;
                            }
                        }
                    }

                    var dummy = new GameObject("RevivePoint_Permanent_Fallback");
                    dummy.transform.position = new Vector3(93.11f, 0f, -100.17f);
                    _revivePoint = dummy.transform;
                }
                return _revivePoint;
            }
            private set => _revivePoint = value;
        }

        public IReadOnlyAsyncReactiveProperty<Invoker> Revived => _revived;

        public void InitRevivePoint(Transform revivePoint)
        {
            Debug.Log($"[MainCharacterGlobalReviveHandler] InitRevivePoint set to: {revivePoint.name} at {revivePoint.position}");
            RevivePoint = revivePoint;
        }

        public void Revive()
        {
            _revived.Invoke();
        }
    }
}
