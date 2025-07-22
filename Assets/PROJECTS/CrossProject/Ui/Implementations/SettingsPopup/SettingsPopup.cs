using CrossProject.Ui.Core;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace CrossProject.Ui.Implementations.SettingsPopup
{
    public class SettingsPopup : PopupView<SettingsPopupModel>
    {
        [SerializeField]
        private Button closePanel;

        [SerializeField]
        private Button musicButton;

        [SerializeField]
        private Button zoom20;

        [SerializeField]
        private Button zoom25;

        [SerializeField]
        private Button zoom30;

        [SerializeField]
        private Button sfxButton;

        [SerializeField]
        private TMP_Dropdown languageSelect;

        [SerializeField]
        private Button discordButton;

        [SerializeField]
        private Button youtubeButton;

        [SerializeField]
        private Button vkButton;

        protected override void OnBind()
        {
            closePanel.onClick.AddListener(() => { Model.Close?.Invoke(); });
            musicButton.onClick.AddListener(() => { Model.ToggleBGM?.Invoke(); });
            sfxButton.onClick.AddListener(() => { Model.ToggleSFX?.Invoke(); });
            zoom20.onClick.AddListener(() => Model.SetZoom?.Invoke(20));
            zoom25.onClick.AddListener(() => Model.SetZoom?.Invoke(25));
            zoom30.onClick.AddListener(() => Model.SetZoom?.Invoke(30));
            languageSelect.onValueChanged.AddListener(x => { Debug.Log($"Selected option : {x}"); });
            discordButton.onClick.AddListener(() => Application.OpenURL("https://discord.gg"));
            youtubeButton.onClick.AddListener(() => Application.OpenURL("https://youtube.com"));
            vkButton.onClick.AddListener(() => Application.OpenURL("https://vk.com"));
        }
    }
}