using CrossProject.Ui.Core;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using R3;

namespace CrossProject.Ui.Implementations.SettingsPopup
{
    public class SettingsPopup : PopupView<SettingsPopupModel>
    {
        [SerializeField]
        private Button _musicButton;

        [SerializeField]
        private Button _sfxButton;

        [SerializeField]
        private TMP_Dropdown _languageSelect;

        [SerializeField]
        private Button _discordButton;

        [SerializeField]
        private Button _youtubeButton;

        [SerializeField]
        private Button _vkButton;

        private void Awake()
        {
            _musicButton.onClick.AddListener(() => { Debug.Log("ToggleMusic"); });
            _sfxButton.onClick.AddListener(() => { Debug.Log("ToggleSFX"); });
            _discordButton.onClick.AddListener(() => { Debug.Log("ToggleDiscord"); });
            _youtubeButton.onClick.AddListener(() => { Debug.Log("ToggleYouTube"); });
            _vkButton.onClick.AddListener(() => { Debug.Log("ToggleVK"); });
        }
    }
}