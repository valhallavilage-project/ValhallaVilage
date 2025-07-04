using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Core.Camera
{
    public class CameraService : MonoBehaviour, IInitializable, IPostLateTickable
    {
        private Transform _target;

        [SerializeField] private Transform zoomHandle;
        [SerializeField] private CameraConfig cameraConfig;

        public float XRotation
        {
            get => transform.rotation.eulerAngles.x;
            set
            {
                var euler = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(value, euler.y, euler.z);
            }
        }

        public float YRotation
        {
            get => transform.rotation.eulerAngles.y;
            set
            {
                var euler = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(euler.x, value, euler.z);
            }
        }

        public float Zoom
        {
            get => zoomHandle.localPosition.z;
            set => zoomHandle.localPosition = new Vector3(0, 0, value);
        }

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
            YRotation = cameraConfig.yRotationAngle;
            XRotation = cameraConfig.xRotationAngle;
            Zoom = cameraConfig.zoomDistance;
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
    }
}