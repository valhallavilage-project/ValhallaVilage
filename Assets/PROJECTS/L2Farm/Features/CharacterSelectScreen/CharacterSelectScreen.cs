using CrossProject.Core.Characters;
using CrossProject.Ui.Core;
using UnityEngine.UI;
using UnityEngine;

namespace PROJECTS.L2Farm.Scripts.CharacterSkinSelect
{
    public class CharacterSelectScreen : ScreenView<CharacterSelectScreenModel>
    {
        [SerializeField]
        private Button femaleButton;

        [SerializeField]
        private GameObject femaleCheck;

        [SerializeField]
        private CharacterId femaleSkin;

        [SerializeField]
        private Button maleButton;

        [SerializeField]
        private GameObject maleCheck;

        [SerializeField]
        private CharacterId maleSkin;

        [SerializeField]
        private Button nextButton;

        private void SelectCharacter(bool first)
        {
            femaleCheck.SetActive(first);
            maleCheck.SetActive(!first);
            nextButton.gameObject.SetActive(true);
            var characterId = first
                ? femaleSkin
                : maleSkin;
            Model.OnCharacterSelected?.Invoke(characterId);
        }

        private void Awake()
        {
            femaleButton.onClick.AddListener(() => {
                SelectCharacter(true);
            });
            maleButton.onClick.AddListener(() => {
                SelectCharacter(false);
            });
            nextButton.onClick.AddListener(() => {
                Model.Close?.Invoke();
            });
        }
    }
}