using System.Collections.Generic;
using System.Linq;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace CrossProject.Core
{
    public interface IMainCharacterClothesSetsService
    {
        int GetTotalHealth();
        int GetTotalEnergy();
        int GetTotalDamage();
    }

    public class MainCharacterClothesSetsService : IInitializable, IMainCharacterClothesSetsService
    {
        private readonly GameStateManager _gameStateManager;
        private readonly IHealthHandler _healthHandler;
        private readonly IEnergyHandler _energyHandler;
        private readonly Dictionary<MainCharacterClothesSetType, MainCharacterClothesSetConfig> _clothesSetConfigs;
        private WornClothesSet _wornClothesSet;

        public bool IsInitialized { get; private set; }

        public MainCharacterClothesSetsService(GameStateManager gameStateManager, MainCharacterClothesSetConfigFacade clothesSetConfigs, IHealthHandler healthHandler, IEnergyHandler energyHandler)
        {
            _gameStateManager = gameStateManager;
            _healthHandler = healthHandler;
            _energyHandler = energyHandler;
            _clothesSetConfigs = clothesSetConfigs.Configs.ToDictionary(c => c.ClothesSetType, c => c);
        }

        public UniTask Initialize()
        {
            if (!_gameStateManager.State.TryGet(out _wornClothesSet))
                _wornClothesSet = _gameStateManager.State.Set(new WornClothesSet());

            _healthHandler.Init(GetTotalHealth(), GetTotalHealth());
            _energyHandler.Init(GetTotalEnergy(), GetTotalEnergy());

            IsInitialized = true;

            return UniTask.CompletedTask;
        }

        public int GetTotalHealth()
        {
            return _clothesSetConfigs[MainCharacterClothesSetType.Default].Health +
                _wornClothesSet.ClothesSet != MainCharacterClothesSetType.Default
                    ? _clothesSetConfigs[_wornClothesSet.ClothesSet].Health
                    : 0;
        }

        public int GetTotalEnergy()
        {
            return _clothesSetConfigs[MainCharacterClothesSetType.Default].Energy +
                _wornClothesSet.ClothesSet != MainCharacterClothesSetType.Default
                    ? _clothesSetConfigs[_wornClothesSet.ClothesSet].Energy
                    : 0;
        }

        public int GetTotalDamage()
        {
            return _clothesSetConfigs[MainCharacterClothesSetType.Default].Damage +
                _wornClothesSet.ClothesSet != MainCharacterClothesSetType.Default
                    ? _clothesSetConfigs[_wornClothesSet.ClothesSet].Damage
                    : 0;
        }
    }
}
