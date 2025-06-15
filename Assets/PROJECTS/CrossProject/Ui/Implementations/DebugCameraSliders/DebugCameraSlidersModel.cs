using System.Collections.Generic;
using CrossProject.Ui.Core;
using CrossProject.Ui.Implementations.Slider;

namespace CrossProject.Ui.Implementations.DebugCameraSliders
{
    public class DebugCameraSlidersModel : HudElementModel
    {
        public List<SliderModel> SliderModels = new();
    }
}