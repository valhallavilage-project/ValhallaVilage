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
    public class SettingsPopupController : IInitializable
    {
        private readonly UiService _uiService;
        private readonly CameraService _cameraService;
        private readonly AudioManager _audioManager;

        private SettingsHudElement _settingsHudElement;
        private SettingsPopup _popupView;

        public bool IsInitialized { get; private set; }

        public SettingsPopupController(
            UiService uiService,
            CameraService cameraService,
            AudioManager audioManager)
        {
            _uiService = uiService;
            _cameraService = cameraService;
            _audioManager = audioManager;
        }

        public async UniTask Initialize()
        {
            var hudElementModel = new SettingsHudElementModel();
            _settingsHudElement = await _uiService.TryOpen(hudElementModel) as SettingsHudElement;
            if (_settingsHudElement == null)
                throw new Exception($"Something went wrong while opening {nameof(SettingsHudElement)}");
            _settingsHudElement.OnSettingsHudElementClick += OpenSettings;
            IsInitialized = true;
        }

        private SettingsPopupModel GetPopupModel()
        {
            var result = new SettingsPopupModel
            {
                Close = () => _uiService.Close(_popupView),
                SetZoom = x => _cameraService.Zoom = x,
                ToggleBGM = _audioManager.ToggleBackgroundTrack,
                ToggleSFX = _audioManager.ToggleSfx
            };
            return result;
        }

        private async void OpenSettings()
        {
            _popupView = await _uiService.TryOpen(GetPopupModel()) as SettingsPopup;
        }
    }
}