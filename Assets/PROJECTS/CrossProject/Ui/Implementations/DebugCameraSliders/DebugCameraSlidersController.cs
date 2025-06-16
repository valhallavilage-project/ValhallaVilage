using System.Collections.Generic;
using CrossProject.Core.Camera;
using CrossProject.Ui.Core;
using CrossProject.Ui.Implementations.Slider;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace CrossProject.Ui.Implementations.DebugCameraSliders
{
    //TODO : VM : consider adding abstract UiController class with UiService
    public class DebugCameraSlidersController : IInitializable
    {
        private readonly UiService _uiService;
        private readonly CameraService _cameraService;

        public DebugCameraSlidersController(
            UiService uiService,
            CameraService cameraService)
        {
            _uiService = uiService;
            _cameraService = cameraService;
        }

        private void InvertCameraXRotation(float value)
        {
            _cameraService.SetXRotation(value * -1);
        }

        public void Initialize()
        {
            _uiService.TryOpen(new DebugCameraSlidersModel
            {
                SliderModels = new List<SliderModel>
                {
                    new()
                    {
                        OnSliderChange = _cameraService.SetYRotation,
                        MaxValue = 360,
                        DefaultValue = 0,
                        WholeNumbers = true
                    },
                    new()
                    {
                        OnSliderChange = InvertCameraXRotation,
                        MinValue = -360,
                        MaxValue = -270,
                        DefaultValue = -315,
                        WholeNumbers = true
                    },
                    new()
                    {
                        OnSliderChange = _cameraService.SetZoom,
                        WholeNumbers = true
                    }
                }
            }).Forget();
        }
    }
}