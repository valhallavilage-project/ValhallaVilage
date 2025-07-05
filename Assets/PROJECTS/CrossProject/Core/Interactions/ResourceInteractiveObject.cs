using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using UnityEngine;
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

        public override async UniTask Interaction()
        {
            await UniTask.WaitForSeconds(interactionDuration);
            Respawn().Forget();
        }

        private async UniTask Respawn()
        {
            viewRoot.SetActive(false);
            CanInteract = false;
            await UniTask.WaitForSeconds(15);
            viewRoot.SetActive(true);
            CanInteract = true;
        }
    }
}