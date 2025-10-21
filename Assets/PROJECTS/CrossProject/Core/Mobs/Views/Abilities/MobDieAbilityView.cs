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
            dieAbility.DeathBegan.Listen(MobBeganToDie, gameObject.GetCancellationTokenOnDestroy());
            dieAbility.DeathCompleted.Listen(MobDie, gameObject.GetCancellationTokenOnDestroy());
        }

        private void MobBeganToDie()
        {
            _mainCharacterGlobalExperienceGainHandler.GainXp(_config.ExperienceReward);
        }

        private void MobDie()
        {
            var corpse = Instantiate(_corpsePrefab, transform.position, transform.rotation);
            
            corpse.StartDecay(_config.CorpseDecayTime);
        }
    }
}