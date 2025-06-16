using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Core.Camera
{
    public class CameraService : MonoBehaviour, IInitializable, IPostLateTickable
    {
        private Transform _target;

        [SerializeField] private Transform zoomHandle;
        [SerializeField] private CameraConfig cameraConfig;

        public Vector3 CamDirectionOnPlane
        {
            get
            {
                var direction = transform.position - zoomHandle.position;
                direction.y = 0;
                return direction;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(zoomHandle.position, 1);
            Gizmos.DrawLine(zoomHandle.position, zoomHandle.position + CamDirectionOnPlane);
            Gizmos.color = Color.white;
            Gizmos.DrawSphere(zoomHandle.position + CamDirectionOnPlane, 1);
        }

        public void Initialize()
        {
            DontDestroyOnLoad(gameObject);
            SetXRotation(cameraConfig.xRotationAngle);
            SetYRotation(cameraConfig.yRotationAngle);
            SetZoom(cameraConfig.zoomDistance);
        }

        public void PostLateTick()
        {
            if (_target == null)
                return;

            transform.position = _target.position;
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
            var euler = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(euler.x, angle, euler.z);
        }

        public void SetXRotation(float angle)
        {
            var euler = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(angle, euler.y, euler.z);
        }
    }
}