using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using VContainer;

namespace CrossProject.Core.Interactions
{
    public class ResourceInteractiveObject : InteractiveObject
    {
        private GameStateManager _gameStateManager;

        [Inject]
        private void Construct(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }

        public override async UniTask Interact()
        {
            await UniTask.WaitForSeconds(interactionDuration);
            Destroy(gameObject);
        }
    }
}