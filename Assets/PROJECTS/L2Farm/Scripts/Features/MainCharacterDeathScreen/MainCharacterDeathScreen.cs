using CrossProject.Ui.Core;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm.Features
{
    public class MainCharacterDeathScreen : ScreenView<MainCharacterDeathScreenModel>
    {
        [SerializeField] private Button _reviveButton;

        protected override void OnBind()
        {
            _reviveButton.SetUniqueCallback(Model.Close);
            _reviveButton.onClick.AddListener(ReviveMainCharacter);
        }

        private void ReviveMainCharacter()
        {
            Model.MainCharacterReviveHandler.Revive();
        }
    }
}
