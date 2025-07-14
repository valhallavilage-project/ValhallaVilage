using System;
using CrossProject.Ui.Core;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Ui.Implementations.SettingsPopup
{
    public class SettingsPopupController : IInitializable
    {
        private readonly UiService _uiService;

        private SettingsHudElement _settingsHudElement;
        private SettingsPopup _popupView;

        public SettingsPopupController(UiService uiService)
        {
            _uiService = uiService;
        }

        public async void Initialize()
        {
            var hudElementModel = new SettingsHudElementModel()
            {
                anchorMin = Vector2.one,
                anchorMax = Vector2.one,
                pivot = Vector2.one,
                
            };
            _settingsHudElement = await _uiService.TryOpen(hudElementModel) as SettingsHudElement;
            if (_settingsHudElement == null)
                throw new Exception($"Something went wrong while opening {nameof(SettingsHudElement)}");
            _settingsHudElement.OnSettingsHudElementClick += OpenSettings;
        }

        private SettingsPopupModel GetPopupModel()
        {
            var result = new SettingsPopupModel
            {
                Close = () => _uiService.Close(_popupView)
            };
            return result;
        }

        private async void OpenSettings()
        {
            _popupView = await _uiService.TryOpen(GetPopupModel()) as SettingsPopup;
        }
    }
}