using System;
using CrossProject.Ui.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CrossProject.Ui.Implementations.SettingsPopup
{
    public class SettingsHudElement : HudElementView<SettingsHudElementModel>
    {
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private Button button;

        public event Action OnSettingsHudElementClick;

        private void EventInvocation()
        {
            OnSettingsHudElementClick?.Invoke();
        }

        private void Awake()
        {
            button.onClick.AddListener(EventInvocation);
        }

        protected override void OnBind()
        {
            rectTransform.anchorMin = Model.anchorMin;
            rectTransform.anchorMax = Model.anchorMax;
            rectTransform.pivot = Model.pivot;
        }
    }
}