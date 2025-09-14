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
        [SerializeField] private Image frame;
        [SerializeField] private Sprite premiumFrame;

        private IMainCharacterSharedDataHolder _mainCharacterSharedData;

        public void SetPortrait(Sprite sprite) => portrait.sprite = sprite;

        private void Start()
        {
            Injector.Instance.Inject(this);
        }

        [Inject]
        private void Construct(IMainCharacterSharedDataHolder mainCharacterSharedData)
        {
            _mainCharacterSharedData = mainCharacterSharedData;

            mainCharacterSharedData.CurrentEnergy.ForEachAsync(OnEnergyChanged, gameObject.GetCancellationTokenOnDestroy()).Forget();
            mainCharacterSharedData.CurrentHealth.ForEachAsync(OnHealthChanged, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void OnEnergyChanged(float current)
        {
            manaBarFill.fillAmount = current / _mainCharacterSharedData.MaxEnergy.Value;
            manaLabel.text = $"{Mathf.RoundToInt(current)}/{_mainCharacterSharedData.MaxEnergy.Value}";
        }

        private void OnHealthChanged(float current)
        {
            healthBarFill.fillAmount = current / _mainCharacterSharedData.MaxHealth.Value;
            _healthLabel.text = $"{Mathf.RoundToInt(current)}/{_mainCharacterSharedData.MaxHealth.Value}";
        }
    }
}
