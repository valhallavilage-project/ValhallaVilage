using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Core.Camera
{
    public class CameraService : MonoBehaviour, IInitializable, IPostLateTickable
    {
        private Transform _transform;
        private Transform _target;

        [SerializeField] private Transform zoomHandle;

        public void Initialize()
        {
            _transform = transform;
            DontDestroyOnLoad(gameObject);
        }

        public void PostLateTick()
        {
            if (_target == null)
                return;

            _transform.position = _target.position;
        }

        public void SetTarget(Transform target)
        {
            _target = target;
        }

        public void SetZoom(float distance)
        {
            zoomHandle.localPosition = new Vector3(0, 0, distance);
        }

        public void SetYRotation(float angle)
        {
            _transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        public void SetXRotation(float angle)
        {
            _transform.rotation = Quaternion.Euler(angle, 0, 0);
        }
    }
}