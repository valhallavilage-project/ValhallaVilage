using System;
using System.Threading;
using CrossProject.Core.Audio;
using CrossProject.Core.Camera;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Ui.Implementations.SettingsPopup
{
    public class SettingsPopupController : IAsyncStartable
    {
        private readonly UiService _uiService;
        private readonly CameraService _cameraService;
        private readonly AudioManager _audioManager;

        private SettingsHudElement _settingsHudElement;
        private SettingsPopup _popupView;

        public SettingsPopupController(
            UiService uiService,
            CameraService cameraService,
            AudioManager audioManager)
        {
            _uiService = uiService;
            _cameraService = cameraService;
            _audioManager = audioManager;
        }

        public async UniTask StartAsync(CancellationToken cancellation = default)
        {
            var hudElementModel = new SettingsHudElementModel();
            _settingsHudElement = await _uiService.TryOpen(hudElementModel) as SettingsHudElement;
            if (_settingsHudElement == null)
                throw new Exception($"Something went wrong while opening {nameof(SettingsHudElement)}");
            _settingsHudElement.OnSettingsHudElementClick += OpenSettings;
        }

        private SettingsPopupModel GetPopupModel()
        {
            var result = new SettingsPopupModel
            {
                Close = () => _uiService.Close(_popupView),
                SetZoom = x => _cameraService.Zoom = x,
                ToggleBGM = _audioManager.ToggleBGM,
                ToggleSFX = _audioManager.ToggleSFX
            };
            return result;
        }

        private async void OpenSettings()
        {
            _popupView = await _uiService.TryOpen(GetPopupModel()) as SettingsPopup;
        }
    }
}