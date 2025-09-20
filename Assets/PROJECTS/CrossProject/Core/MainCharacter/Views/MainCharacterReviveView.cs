using CrossProject.Core.SimpleMovement;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class MainCharacterReviveView : MonoBehaviour
    {
        [SerializeField] private Collider _mainCollider;
        [SerializeField] private Collider _interactionCollider;

        private IInteractionHandler _interactionHandler;
        private SimpleMovementController _simpleMovementController;
        private IReviveAbility _reviveAbility;

        private IMainCharacterReviveGlobalHandler _mainCharacterReviveGlobalHandler;

        [Inject]
        private void AddDependencies(IInteractionHandler interactionHandler, SimpleMovementController simpleMovementController,
            IReviveAbility reviveAbility)
        {
            _interactionHandler = interactionHandler;
            _simpleMovementController = simpleMovementController;
            reviveAbility.Revived.WithoutCurrent().ForEachAsync(Revived, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void Revived(Vector3 _)
        {
            _mainCollider.gameObject.SetActive(true);
            _interactionCollider.gameObject.SetActive(true);

            _interactionHandler.RemoveBlock(_interactionHandler.GetType());
            _simpleMovementController.RemoveBlock(_simpleMovementController.GetType());
        }
    }
}
