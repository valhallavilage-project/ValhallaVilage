using CrossProject.Core;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace L2Farm.Scripts.CharacterHudElement
{
    public class CharacterHudElement : HudElementView<CharacterHudElementModel>
    {
        [SerializeField] private Image portrait;
        [SerializeField] private Image healthBarFill;
        [SerializeField] private TMP_Text _healthLabel;
        [SerializeField] private Image manaBarFill;
        [SerializeField] private TMP_Text manaLabel;
        [SerializeField] private Image _experienceBarFill;
        [SerializeField] private TMP_Text _levelLabel;

        private IMainCharacterGlobalFacade _mainCharacterSharedData;

        public void SetPortrait(Sprite sprite) => portrait.sprite = sprite;

        private void Start()
        {
            Injector.Instance.Inject(this);
        }

        [Inject]
        private void Construct(IMainCharacterGlobalFacade mainCharacterSharedData)
        {
            _mainCharacterSharedData = mainCharacterSharedData;

            mainCharacterSharedData.CurrentEnergy.ForEachAsync(ChangeEnergy, gameObject.GetCancellationTokenOnDestroy()).Forget();
            mainCharacterSharedData.CurrentHealth.ForEachAsync(ChangeHealth, gameObject.GetCancellationTokenOnDestroy()).Forget();
            mainCharacterSharedData.CurrentExperience.ForEachAsync(ChangeExperience, gameObject.GetCancellationTokenOnDestroy()).Forget();
            mainCharacterSharedData.CurrentLevel.ForEachAsync(ChangeLevel, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void ChangeEnergy(float current)
        {
            manaBarFill.fillAmount = current / _mainCharacterSharedData.MaxEnergy.Value;
            manaLabel.text = $"{Mathf.RoundToInt(current)}/{_mainCharacterSharedData.MaxEnergy.Value}";
        }

        private void ChangeHealth(float current)
        {
            healthBarFill.fillAmount = current / _mainCharacterSharedData.MaxHealth.Value;
            _healthLabel.text = $"{Mathf.RoundToInt(current)}/{_mainCharacterSharedData.MaxHealth.Value}";
        }

        private void ChangeExperience(float current)
        {
            current -= _mainCharacterSharedData.MinExperience.Value;

            var progress = _mainCharacterSharedData.MaxExperience.Value - _mainCharacterSharedData.MinExperience.Value; 
            
            _experienceBarFill.fillAmount = current / progress;
        }

        private void ChangeLevel(int current)
        {
            _levelLabel.text = current.ToString();
        }
    }
}
