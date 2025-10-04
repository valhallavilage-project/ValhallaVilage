using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class LevelUpAbilityView : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _levelUpParticles;
        
        private IHealthHandler _healthHandler;
        private IEnergyHandler _energyHandler;
        private IExperienceHandler _experienceHandler;

        [Inject]
        private void AddDependencies(IExperienceHandler experienceHandler, IHealthHandler healthHandler, IEnergyHandler energyHandler)
        {
            _healthHandler = healthHandler;
            _energyHandler = energyHandler;
            _experienceHandler = experienceHandler;
            
            experienceHandler.CurrentLevel.WithoutCurrent().ForEachAsync(LevelChanged, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void LevelChanged(int _)
        {
            if (!_experienceHandler.IsInitialized)
            {
                return;
            }
            
            _healthHandler.Restore(_healthHandler.MaxHealth.Value);
            _energyHandler.Restore(_energyHandler.MaxEnergy.Value);
            
            _levelUpParticles.Play();
        }
    }
}
