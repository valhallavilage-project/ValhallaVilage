using CrossProject.Ui.Core;
using UnityEngine;

namespace CrossProject.Ui.Implementations.DebugCameraSliders
{
    public class DebugCameraSliders : HudElementView<DebugCameraSlidersModel>
    {
        [SerializeField] private Transform rootForSliders;
        //TODO : VM : this is lazy implementations for debug - do not do this in production
        [SerializeField] private Slider.Slider sliderPrefab;

        protected override async void OnBind()
        {
            foreach (var sliderModel in Model.SliderModels)
            {
                var slider = Instantiate(sliderPrefab, rootForSliders);
                slider.BindModel(sliderModel);
            }
        }
    }
}