using System;
using System.Threading;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using VContainer.Unity;

namespace CrossProject.Core
{
    public class MainCharacterSaveHealthHandler : IInitializable, IDisposable
    {
        private readonly IRestoreEnergyHandler _restoreEnergyHandler;
        private readonly IHealthHandler _healthHandler;
        private readonly GameStateManager _gameStateManager;
        private readonly IMainCharacterClothesSetsService _clothesSetsService;
        private readonly CancellationTokenSource _disposeCts = new();

        public bool IsInitialized { get; private set; }

        public MainCharacterSaveHealthHandler(IHealthHandler healthHandler, GameStateManager gameStateManager,
            IMainCharacterClothesSetsService clothesSetsService)
        {
            _healthHandler = healthHandler;
            _gameStateManager = gameStateManager;
            _clothesSetsService = clothesSetsService;
        }

        public UniTask Initialize()
        {
            if (!_gameStateManager.State.TryGet<HealthStatePart>(out var healthStatePart))
            {
                healthStatePart = new HealthStatePart
                {
                    Value = _clothesSetsService.GetTotalHealth()
                };

                _gameStateManager.State.Set(healthStatePart);
                _gameStateManager.Save();
            }

            _healthHandler.Init(_clothesSetsService.GetTotalHealth(), healthStatePart.Value);
            
            _healthHandler.Health.WithoutCurrent().ForEachAsync(HealthChanged, _disposeCts.Token).Forget();

            IsInitialized = true;

            return UniTask.CompletedTask;
        }

        private void HealthChanged(float newValue)
        {
            var healthStatePart =_gameStateManager.State.Get<HealthStatePart>();

            healthStatePart.Value = newValue;

            _gameStateManager.Save();
        }

        public void Dispose()
        {
            _disposeCts?.Dispose();
        }
    }
}
