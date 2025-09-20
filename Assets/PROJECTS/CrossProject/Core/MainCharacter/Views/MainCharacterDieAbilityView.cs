using CrossProject.Core.SimpleMovement;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class MainCharacterDieAbilityView : MonoBehaviour
    {
        [SerializeField] private Collider _mainCollider;
        [SerializeField] private Collider _interactionCollider;

        private IInteractionHandler _interactionHandler;
        private SimpleMovementController _simpleMovementController;

        [Inject]
        private void AddDependencies(IInteractionHandler interactionHandler, SimpleMovementController simpleMovementController,
            IDieAbility dieAbility)
        {
            _interactionHandler = interactionHandler;
            _simpleMovementController = simpleMovementController;
            
            dieAbility.DeathBegan.WithoutCurrent().ForEachAsync(Died, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void Died(bool _)
        {
            _mainCollider.gameObject.SetActive(false);
            _interactionCollider.gameObject.SetActive(false);
            
            _interactionHandler.AddBlock(_interactionHandler.GetType());
            _simpleMovementController.AddBlock(_simpleMovementController.GetType());
        }
    }
}
