using System;
using CrossProject.Ui.Core;
using UnityEngine;
using UnityEngine.UI;

namespace CrossProject.Ui.Implementations.SettingsPopup
{
    public class SettingsHudElement : HudElementView<SettingsHudElementModel>
    {
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
    }
}