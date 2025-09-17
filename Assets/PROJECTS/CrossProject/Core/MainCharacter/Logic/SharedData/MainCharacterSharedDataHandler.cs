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
        private readonly IExperienceHandler _experienceHandler;
        private CancellationTokenSource _disposeCts = new();
        
        public bool IsInitialized { get; private set; }

        public MainCharacterSharedDataHandler(IMainCharacterSharedDataHolder holder, IHealthHandler healthHandler, IEnergyHandler energyHandler,
            IExperienceHandler experienceHandler, IMainCharacterGlobalExperienceGainHandler experienceGainHandler)
        {
            _experienceHandler = experienceHandler;
            
            healthHandler.Health.ForEachAsync(v => holder.CurrentHealth.Value = v, _disposeCts.Token).Forget();
            healthHandler.MaxHealth.ForEachAsync(v => holder.MaxHealth.Value = v, _disposeCts.Token).Forget();

            energyHandler.Energy.ForEachAsync(v => holder.CurrentEnergy.Value = v, _disposeCts.Token).Forget();
            energyHandler.MaxEnergy.ForEachAsync(v => holder.MaxEnergy.Value = v, _disposeCts.Token).Forget();

            experienceHandler.CurrentExperience.ForEachAsync(v => holder.CurrentExperience.Value = v, _disposeCts.Token).Forget();
            experienceHandler.MaxExperience.ForEachAsync(v => holder.MaxExperience.Value = v, _disposeCts.Token).Forget();
            experienceHandler.CurrentLevel.ForEachAsync(v => holder.CurrentLevel.Value = v, _disposeCts.Token).Forget();
            
            experienceGainHandler.ExperienceGained.WithoutCurrent().ForEachAsync(GainXp, _disposeCts.Token).Forget();
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

        public void Dispose()
        {
            _disposeCts.Cancel();
            _disposeCts?.Dispose();
        }
    }
}
