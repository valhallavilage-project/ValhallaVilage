using System;
using CrossProject.Core;
using CrossProject.Core.Characters;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using CrossProject.Core.Energy;
using UnityEngine;
using VContainer.Unity;

namespace L2Farm.Scripts.CharacterHudElement
{
    public class CharacterHudElementController : IInitializable, IEnergyProvider
    {
        private readonly AddressablesManager _addressablesManager;
        private readonly UiService _uiService;
        private readonly CharactersService _charactersService;
        private readonly GameStateManager _gameStateManager;

        private CharacterHudElement _view;

        public bool IsInitialized { get; private set; }
        public int CurrentValue { get; private set; } = 100;
        public int MaxValue => 100;

        public event Action<int, int> OnEnergySpend;

        public CharacterHudElementController(
            AddressablesManager addressablesManager,
            UiService uiService,
            CharactersService charactersService,
            GameStateManager gameStateManager)
        {
            _addressablesManager = addressablesManager;
            _uiService = uiService;
            _charactersService = charactersService;
            _gameStateManager = gameStateManager;
        }

        public async UniTask Initialize()
        {
            var energyRestorationConfig = await _addressablesManager.LoadAssetAsync<EnergyRestorationConfig>();
            _view = await _uiService.TryOpen(new CharacterHudElementModel()) as CharacterHudElement;
            if (_gameStateManager.State.TryGet<ObtainedCharactersPart>(out var part))
            {
                var config = _charactersService.GetConfigFor(part.CurrentCharacterId);
                _view.SetPortrait(config.portrait);
                var energyStatePart = _gameStateManager.State.Get<EnergyStatePart>();
                if (energyStatePart.energyValue > 0)
                {
                    int timesToRestore = (int)(DateTime.UtcNow - energyStatePart.lastEnergySpent).TotalSeconds / energyRestorationConfig.intervalInSeconds;
                    var energyToRestore = Mathf.Clamp(energyStatePart.energyValue + timesToRestore * energyRestorationConfig.energyToRestoreForOneInterval, 0, energyRestorationConfig.energyMaximum);
                    CurrentValue = energyToRestore;
                }
            }
            else
            {
                _charactersService.OnCharacterSelected += OnCharacterSelected;
            }
            IsInitialized = true;
        }

        private void OnCharacterSelected(CharacterId characterId)
        {
            var config = _charactersService.GetConfigFor(characterId);
            _view.SetPortrait(config.portrait);
        }

        public void Spend(int amount)
        {
            if (amount > CurrentValue || amount <= 0)
                return;

            CurrentValue -= amount;
            _view.SetMana(CurrentValue / (float)MaxValue);
            OnEnergySpend?.Invoke(amount, CurrentValue);
        }
    }
}
