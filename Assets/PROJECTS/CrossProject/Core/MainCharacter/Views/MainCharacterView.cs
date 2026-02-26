using CrossProject.Core.SimpleMovement;
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
        private SimpleMovementController _simpleMovementController;

        [Inject]
        private void AddDependencies(IReviveAbility reviveAbility, IDieAbility dieAbility, IHealthHandler healthHandler,
            SimpleMovementController simpleMovementController)
        {
            _dieAbility = dieAbility;
            _healthHandler = healthHandler;
            _simpleMovementController = simpleMovementController;
            
            healthHandler.Health.WithoutCurrent().ForEachAsync(HealthChanged, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void HealthChanged(float _)
        {
            if (_healthHandler.IsDied)
            {
                _dieAbility.BeginToDie();
            }
        }
    }
}
