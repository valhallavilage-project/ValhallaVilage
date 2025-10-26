using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Core
{
    public interface IMainCharacterArmorSetsService : IInitializable
    {
        IReadOnlyAsyncReactiveProperty<MainCharacterArmorSetType> ArmorSetChanged { get; }
        
        int GetTotalHealth();
        int GetTotalEnergy();
        int GetTotalDamage();
        bool IsSetAvailable(MainCharacterArmorSetType setType);
    }

    public class MainCharacterArmorSetsService : IMainCharacterArmorSetsService, IDisposable
    {
        private readonly GameStateManager _gameStateManager;
        private readonly IExperienceHandler _experienceHandler;
        private readonly Dictionary<MainCharacterArmorSetType, MainCharacterSpecificArmorSetConfig> _armorSetConfigs;
        private WornArmorSet _wornArmorSet;
        private readonly CancellationTokenSource _disposeCts = new();

        private readonly AsyncReactiveProperty<MainCharacterArmorSetType> _armorSetChanged = new(default);
        private IHealthHandler _healthHandler;
        private IEnergyHandler _energyHandler;

        public IReadOnlyAsyncReactiveProperty<MainCharacterArmorSetType> ArmorSetChanged => _armorSetChanged;

        public bool IsInitialized { get; private set; }

        public MainCharacterArmorSetsService(GameStateManager gameStateManager, 
            MainCharacterArmorSetsConfig armorSetsConfig, IHealthHandler healthHandler, IEnergyHandler energyHandler, IExperienceHandler experienceHandler,
            IMainCharacterGlobalArmorSetChangeHandler globalArmorSetChangeHandler)
        {
            _energyHandler = energyHandler;
            _healthHandler = healthHandler;
            _gameStateManager = gameStateManager;
            _experienceHandler = experienceHandler;
            _armorSetConfigs = armorSetsConfig.Configs.ToDictionary(c => c.ArmorSetType, c => c);
            
            globalArmorSetChangeHandler.ArmorSetChanged.WithoutCurrent().ForEachAsync(ChangeSet, _disposeCts.Token).Forget();
        }

        public UniTask Initialize()
        {
            if (!_gameStateManager.State.TryGet(out _wornArmorSet))
            {
                _wornArmorSet = _gameStateManager.State.Set(new WornArmorSet());
            }

            IsInitialized = true;

            return UniTask.CompletedTask;
        }

        public int GetTotalHealth()
        {
            return _armorSetConfigs[MainCharacterArmorSetType.Default].Health +
                _wornArmorSet.ArmorSet != MainCharacterArmorSetType.Default
                    ? _armorSetConfigs[_wornArmorSet.ArmorSet].Health
                    : 0;
        }

        public int GetTotalEnergy()
        {
            return _armorSetConfigs[MainCharacterArmorSetType.Default].Energy +
                _wornArmorSet.ArmorSet != MainCharacterArmorSetType.Default
                    ? _armorSetConfigs[_wornArmorSet.ArmorSet].Energy
                    : 0;
        }

        public int GetTotalDamage()
        {
            return _armorSetConfigs[MainCharacterArmorSetType.Default].Damage +
                _wornArmorSet.ArmorSet != MainCharacterArmorSetType.Default
                    ? _armorSetConfigs[_wornArmorSet.ArmorSet].Damage
                    : 0;
        }

        public bool IsSetAvailable(MainCharacterArmorSetType setType)
        {
            return _armorSetConfigs[MainCharacterArmorSetType.Default].Level <= _experienceHandler.CurrentLevel.Value;
        }

        private void ChangeSet(MainCharacterArmorSetType selectedSet)
        {
            if (_wornArmorSet.ArmorSet == selectedSet)
            {
                return;
            }
            
            if (!IsSetAvailable(selectedSet))
            {
                Debug.LogError("Trying to select set not matched character level parameters");
                
                return;
            }
            
            _wornArmorSet.ArmorSet = selectedSet;
            
            _healthHandler.AssignMaxHealth(_armorSetConfigs[_wornArmorSet.ArmorSet].Health);
            _energyHandler.AssignMaxEnergy(_armorSetConfigs[_wornArmorSet.ArmorSet].Energy);

            _armorSetChanged.Value = selectedSet;
        }

        public void Dispose()
        {
            _disposeCts?.Cancel();
            _disposeCts?.Dispose();
        }
    }
}
