using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace CrossProject.Core
{
    public class MainCharacterView : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _navMeshAgent;

        private IReviveAbility _reviveAbility;
        private IDieAbility _dieAbility;
        private IHealthHandler _healthHandler;

        [Inject]
        private void AddDependencies(IReviveAbility reviveAbility, IDieAbility dieAbility, IHealthHandler healthHandler)
        {
            _dieAbility = dieAbility;
            _healthHandler = healthHandler;
            
            healthHandler.Health.WithoutCurrent().ForEachAsync(HealthChanged, gameObject.GetCancellationTokenOnDestroy()).Forget();
            reviveAbility.Revived.WithoutCurrent().ForEachAsync(Revive, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void HealthChanged(float _)
        {
            if (_healthHandler.IsDied)
            {
                _dieAbility.BeginToDie();
            }
        }

        private void Revive(Vector3 position)
        {
            _navMeshAgent.Warp(position);
        }
    }
}
