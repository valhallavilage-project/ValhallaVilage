using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class MobDieAbilityView : MonoBehaviour
    {
        [SerializeField] private MobCorpseView _corpsePrefab;

        private MobConfig _config;
        private IMainCharacterGlobalExperienceGainHandler _mainCharacterGlobalExperienceGainHandler;

        [Inject]
        private void AddDependencies(IDieAbility dieAbility, MobConfig config, IMainCharacterGlobalExperienceGainHandler mainCharacterGlobalExperienceGainHandler)
        {
            _config = config;

            _mainCharacterGlobalExperienceGainHandler = mainCharacterGlobalExperienceGainHandler;
            dieAbility.DeathCompleted.WithoutCurrent().ForEachAsync(MobDie, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void MobDie(bool _)
        {
            _mainCharacterGlobalExperienceGainHandler.GainXp(_config.ExperienceReward);
            
            var corpse = Instantiate(_corpsePrefab, transform.position, transform.rotation);
            
            corpse.StartDecay(_config.CorpseDecayTime);
        }
    }
}