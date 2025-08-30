using CrossProject.Core;
using CrossProject.Core.Energy;
using CrossProject.Ui.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace L2Farm.Scripts.CharacterHudElement
{
    public class CharacterHudElement : HudElementView<CharacterHudElementModel>
    {
        private IEnergyProvider _energyProvider;

        [SerializeField] private Image portrait;

        [SerializeField] private Image healthBarFill;

        [SerializeField] private Image manaBarFill;

        [SerializeField] private TMP_Text manaLabel;

        [SerializeField] private Image frame;

        [SerializeField] private Sprite premiumFrame;

        private void Start()
        {
            Injector.Instance?.Inject(this);
        }

        [Inject]
        private void Construct(
            IEnergyProvider energyProvider)
        {
            _energyProvider = energyProvider;
            _energyProvider.OnEnergyChanged += OnEnergyChanged;
        }

        public void SetPortrait(Sprite sprite) => portrait.sprite = sprite;

        private void OnEnergyChanged(int old, int current)
        {
            manaBarFill.fillAmount = (float)current/_energyProvider.MaxValue;
            manaLabel.text = $"{current}/{_energyProvider.MaxValue}";
        }

        public void SetPremium(bool value)
        {
            if (value)
                frame.sprite = premiumFrame;
        }
    }
}
