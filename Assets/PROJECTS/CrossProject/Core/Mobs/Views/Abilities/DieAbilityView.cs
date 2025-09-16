using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class DieAbilityView : MonoBehaviour
    {
        [SerializeField] private MobCorpseView _corpsePrefab;

        private MobConfig _config;

        [Inject]
        private void AddDependencies(IDieAbility dieAbility, MobConfig config)
        {
            _config = config;
            
            dieAbility.DeathCompleted.WithoutCurrent().ForEachAsync(MobDie, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void MobDie(bool _)
        {
            var corpse = Instantiate(_corpsePrefab, transform.position, transform.rotation);
            
            corpse.StartDecay(_config.CorpseDecayTime);
        }
    }
}