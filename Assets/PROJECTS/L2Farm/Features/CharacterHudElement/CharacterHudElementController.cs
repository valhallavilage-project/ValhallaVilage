using System;
using CrossProject.Core;
using CrossProject.Core.Characters;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using CrossProject.Core.Energy;
using CrossProject.Core.Experience;
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
        private EnergyRestorationConfig _energyRestorationConfig;
        private int _currentValue = 100;
        private int _currentExp = 0;

        public bool IsInitialized { get; private set; }
        public int CurrentValue
        {
            get => _currentValue;

            private set
            {
                int oldValue = _currentValue;
                _currentValue = value;

                var part = _gameStateManager.State.Get<EnergyStatePart>();
                part.energyValue = _currentValue;
                if (oldValue > _currentValue)
                    part.lastEnergySpent = DateTime.Now;
                _gameStateManager.Save();

                OnEnergyChanged?.Invoke(oldValue, _currentValue);
            }
        }

        public int MaxValue => 100;

        public int CurrentTotalExp
        {
            get => _currentExp;
        }

        public int CurrentLevel { get; }

        public event Action<int, int> OnEnergyChanged;

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
            _energyRestorationConfig = await _addressablesManager.LoadAssetAsync<EnergyRestorationConfig>();
            _view = await _uiService.TryOpen(new CharacterHudElementModel()) as CharacterHudElement;

            if (_gameStateManager.State.TryGet<ObtainedCharactersPart>(out var part))
            {
                var config = _charactersService.GetConfigFor(part.CurrentCharacterId);
                _view.SetPortrait(config.portrait);
            }
            else
            {
                _charactersService.OnCharacterSelected += OnCharacterSelected;
            }

            var energyStatePart = _gameStateManager.State.Get<EnergyStatePart>();
            if (energyStatePart.energyValue > 0)
            {
                int timesToRestore = (int)(DateTime.UtcNow - energyStatePart.lastEnergySpent).TotalSeconds / _energyRestorationConfig.intervalInSeconds;
                var energyAmountAfterRestoration = Mathf.Clamp(energyStatePart.energyValue + timesToRestore * _energyRestorationConfig.energyToRestoreForOneInterval, 0, _energyRestorationConfig.energyMaximum);
                CurrentValue = energyAmountAfterRestoration;
            }

            RestorationRoutine().Forget();
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
        }

        private async UniTask RestorationRoutine()
        {
            var interval = TimeSpan.FromSeconds(_energyRestorationConfig.intervalInSeconds);
            while (true)
            {
                await UniTask.Delay(interval);
                CurrentValue += _energyRestorationConfig.energyToRestoreForOneInterval;
            }
        }
    }
}
