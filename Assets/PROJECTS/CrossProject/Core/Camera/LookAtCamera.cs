using CrossProject.Core.Camera;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class LookAtCamera : MonoBehaviour
    {
        private UnityEngine.Camera _camera;

        [Inject]
        public void AddDependencies(CameraService cameraService)
        {
            _camera = cameraService.MainCamera;
        }

        
    }
}
