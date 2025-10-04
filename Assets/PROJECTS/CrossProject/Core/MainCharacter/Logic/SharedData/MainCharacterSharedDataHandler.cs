using System;
using System.Threading;
using Codice.CM.Interfaces;
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
        private readonly IMainCharacterFacade _holder;
        private readonly IHealthHandler _healthHandler;
        private readonly IExperienceHandler _experienceHandler;
        private readonly CancellationTokenSource _disposeCts = new();

        public bool IsInitialized { get; private set; }

        public MainCharacterSharedDataHandler(IMainCharacterFacade holder, IHealthHandler healthHandler, IEnergyHandler energyHandler,
            IExperienceHandler experienceHandler, IMainCharacterGlobalExperienceGainHandler globalExperienceGainHandler,
            IMainCharacterReviveGlobalHandler mainCharacterReviveHandler, IReviveAbility reviveAbility)
        {
            _holder = holder;
            _healthHandler = healthHandler;
            _experienceHandler = experienceHandler;

            healthHandler.Health.ForEachAsync(HealthChanged, _disposeCts.Token).Forget();
            healthHandler.MaxHealth.ForEachAsync(v => holder.MaxHealth.Value = v, _disposeCts.Token).Forget();

            energyHandler.Energy.ForEachAsync(v => holder.CurrentEnergy.Value = v, _disposeCts.Token).Forget();
            energyHandler.MaxEnergy.ForEachAsync(v => holder.MaxEnergy.Value = v, _disposeCts.Token).Forget();

            experienceHandler.CurrentExperience.ForEachAsync(v => holder.CurrentExperience.Value = v, _disposeCts.Token).Forget();
            experienceHandler.MaxExperience.ForEachAsync(v => holder.MaxExperience.Value = v, _disposeCts.Token).Forget();
            experienceHandler.MinExperience.ForEachAsync(v => holder.MinExperience.Value = v, _disposeCts.Token).Forget();
            experienceHandler.CurrentLevel.ForEachAsync(v => holder.CurrentLevel.Value = v, _disposeCts.Token).Forget();

            globalExperienceGainHandler.ExperienceGained.WithoutCurrent().ForEachAsync(GainXp, _disposeCts.Token).Forget();
            
            mainCharacterReviveHandler.Revived.WithoutCurrent().ForEachAsync(_ => reviveAbility.Revive(mainCharacterReviveHandler.RevivePoint.position), _disposeCts.Token).Forget();
        }

        public UniTask Initialize()
        {
            IsInitialized = true;

            return UniTask.CompletedTask;
        }

        private void GainXp(float experience)
        {
            _experienceHandler.GainXp(experience);
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
