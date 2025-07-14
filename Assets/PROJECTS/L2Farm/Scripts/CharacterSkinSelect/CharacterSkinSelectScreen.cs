using CrossProject.Ui.Core;
using UnityEngine.UI;
using UnityEngine;

namespace RUNNER.Assets.PROJECTS.L2Farm.Scripts.CharacterSkinSelect
{
    public class CharacterSkinSelectScreen : ScreenView<CharacterSkinSelectScreenModel>
    {
        [SerializeField]private Button femaleButton;
        [SerializeField]private Button maleButton;
        [SerializeField]private Button nextButton;
        private void Awake()
        {
            femaleButton.onClick.AddListener(() => {
                Debug.Log("female");
            });
            maleButton.onClick.AddListener(() => {
                Debug.Log("male");
            });
            nextButton.onClick.AddListener(() => {
                Debug.Log("next");
            });
        }
    }
}