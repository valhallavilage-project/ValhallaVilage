using System;
using System.Threading;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Core
{
    public class MainCharacterSaveEnergyHandler : IDisposable, IInitializable
    {
        private readonly IRestoreEnergyHandler _restoreEnergyHandler;
        private readonly IEnergyHandler _energyHandler;
        private readonly GameStateManager _gameStateManager;
        private readonly IMainCharacterClothesSetsService _clothesSetsService;
        private readonly EnergyRestorationConfig _energyRestorationConfig;
        private readonly ITimeService _timeService;
        private readonly CancellationTokenSource _disposeCts = new();

        public bool IsInitialized { get; private set; }

        public MainCharacterSaveEnergyHandler(IRestoreEnergyHandler restoreEnergyHandler, IEnergyHandler energyHandler,
            GameStateManager gameStateManager, IMainCharacterClothesSetsService clothesSetsService, EnergyRestorationConfig energyRestorationConfig,
            ITimeService timeService)
        {
            _restoreEnergyHandler = restoreEnergyHandler;
            _energyHandler = energyHandler;
            _gameStateManager = gameStateManager;
            _clothesSetsService = clothesSetsService;
            _energyRestorationConfig = energyRestorationConfig;
            _timeService = timeService;
        }

        public async UniTask Initialize()
        {
            await UniTask.WaitUntil(() => _clothesSetsService.IsInitialized);

            if (!_gameStateManager.State.TryGet<EnergyStatePart>(out var energyStatePart))
            {
                energyStatePart = new EnergyStatePart
                {
                    Value = _clothesSetsService.GetTotalEnergy(),
                    LastRestoreTime = _timeService.Now
                };

                _gameStateManager.State.Set(energyStatePart);
                _gameStateManager.Save();
            }

            var timesToRestore = (float)(_timeService.Now - energyStatePart.LastRestoreTime).TotalSeconds / _energyRestorationConfig.IntervalInSeconds;
            var energyAmountAfterRestoration = Mathf.Clamp(energyStatePart.Value + timesToRestore * _energyRestorationConfig.EnergyToRestoreForOneInterval, 0, _clothesSetsService.GetTotalEnergy());

            _energyHandler.Init(_clothesSetsService.GetTotalEnergy(), energyAmountAfterRestoration);
            
            _energyHandler.Energy.WithoutCurrent().ForEachAsync(EnergyChanged, _disposeCts.Token).Forget();

            IsInitialized = true;
        }

        private void EnergyChanged(float newValue)
        {
            var energyStatePart =_gameStateManager.State.Get<EnergyStatePart>();
            
            energyStatePart.Value = newValue;
            energyStatePart.LastRestoreTime = _restoreEnergyHandler.LastRestoreTime;
            
            _gameStateManager.Save();
        }

        public void Dispose()
        {
            _disposeCts?.Dispose();
        }
    }
}
