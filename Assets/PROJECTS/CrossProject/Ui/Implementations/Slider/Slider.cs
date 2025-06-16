using System.Globalization;
using CrossProject.Ui.Core;
using TMPro;
using UnityEngine;

namespace CrossProject.Ui.Implementations.Slider
{
    public class Slider : HudElementView<SliderModel>
    {
        [SerializeField] private UnityEngine.UI.Slider slider;
        [SerializeField] private TMP_Text label;

        private void OnSliderChange(float value)
        {
            //TODO : VM : check event camera settings in Unity
            Model.OnSliderChange?.Invoke(value);
            label.text = value.ToString(CultureInfo.InvariantCulture);
        }

        protected override void OnBind()
        {
            slider.minValue = Model.MinValue;
            slider.maxValue = Model.MaxValue;
            slider.wholeNumbers = Model.WholeNumbers;
            slider.interactable = Model.Interactable;
            slider.onValueChanged.AddListener(OnSliderChange);
            slider.value = Model.DefaultValue;
        }

        public override void OnClose()
        {
            slider.onValueChanged.RemoveListener(OnSliderChange);
            base.OnClose();
        }
    }
}