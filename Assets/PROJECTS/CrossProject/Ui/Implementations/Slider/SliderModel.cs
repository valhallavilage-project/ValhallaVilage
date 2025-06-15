using System;
using CrossProject.Ui.Core;

namespace CrossProject.Ui.Implementations.Slider
{
    public class SliderModel : HudElementModel
    {
        public float MinValue = 0;
        public float MaxValue = 100;
        public bool WholeNumbers = false;
        public bool Interactable = true;
        public Action<float> OnSliderChange;
    }
}