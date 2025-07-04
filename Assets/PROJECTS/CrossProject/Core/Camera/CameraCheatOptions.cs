using System.ComponentModel;
using CrossProject.Core.Cheats;

namespace CrossProject.Core.Camera
{
    public class CameraCheatOptions : ICheatOption
    {
        private readonly CameraService _cameraService;

        public CameraCheatOptions(CameraService cameraService)
        {
            _cameraService = cameraService;
        }

        [Category("Camera")]
        [DisplayName("Y Rotation")]
        public float YRotation
        {
            get => _cameraService.YRotation;
            set => _cameraService.YRotation = value;
        }

        [Category("Camera")]
        [DisplayName("X Rotation")]
        public float XRotation
        {
            get => _cameraService.XRotation;
            set => _cameraService.XRotation = value;
        }

        [Category("Camera")]
        [DisplayName("Zoom")]
        public float Zoom
        {
            get => _cameraService.Zoom;
            set => _cameraService.Zoom = value;
        }
    }
}