using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using VContainer.Unity;

namespace CrossProject.Core
{
    public interface IMainCharacterSharedDataHandler
    {
    }

    public class MainCharacterSharedDataHandler : IMainCharacterSharedDataHandler, IInitializable, IDisposable
    {
        private readonly IMainCharacterGlobalFacade _holder;
        private readonly IHealthHandler _healthHandler;
        private readonly CancellationTokenSource _disposeCts = new();

        public bool IsInitialized { get; private set; }

        public MainCharacterSharedDataHandler(IMainCharacterGlobalFacade holder, IHealthHandler healthHandler,
            IEnergyHandler energyHandler, IExperienceHandler experienceHandler)
        {
            _holder = holder;
            _healthHandler = healthHandler;

            healthHandler.Health.WithoutCurrent().ForEachAsync(HealthChanged, _disposeCts.Token).Forget();
            healthHandler.MaxHealth.WithoutCurrent().ForEachAsync(v => holder.MaxHealth.Value = v, _disposeCts.Token).Forget();

            energyHandler.Energy.WithoutCurrent().ForEachAsync(v => holder.CurrentEnergy.Value = v, _disposeCts.Token).Forget();
            energyHandler.MaxEnergy.WithoutCurrent().ForEachAsync(v => holder.MaxEnergy.Value = v, _disposeCts.Token).Forget();

            experienceHandler.CurrentExperience.WithoutCurrent().ForEachAsync(v => holder.CurrentExperience.Value = v, _disposeCts.Token).Forget();
            experienceHandler.MaxExperience.WithoutCurrent().ForEachAsync(v => holder.MaxExperience.Value = v, _disposeCts.Token).Forget();
            experienceHandler.MinExperience.WithoutCurrent().ForEachAsync(v => holder.MinExperience.Value = v, _disposeCts.Token).Forget();
            experienceHandler.CurrentLevel.WithoutCurrent().ForEachAsync(v => holder.CurrentLevel.Value = v, _disposeCts.Token).Forget();
        }

        public UniTask Initialize()
        {
            IsInitialized = true;

            return UniTask.CompletedTask;
        }

        private void HealthChanged(float value)
        {
            _holder.CurrentHealth.Value = value;

            if (_healthHandler.IsDied)
            {
                _holder.IsDied.Value = true;
            }
        }

        public void Dispose()
        {
            _disposeCts.Cancel();
            _disposeCts?.Dispose();
        }
    }
}
