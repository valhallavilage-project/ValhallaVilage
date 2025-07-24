using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace CrossProject.Core.Interactions
{
    public class ResourceInteractiveObject : InteractiveObject
    {
        [SerializeField]
        private NavMeshObstacle _obstacle;

        [SerializeField]
        private AudioSource _interactSound;

        private GameStateManager _gameStateManager;

        [Inject]
        private void Construct(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }

        public override async UniTask Interaction()
        {
            _obstacle.enabled = false;
            _interactSound.Play();
            await transform.DOShakeScale(0.2f, new Vector3(0.25f, 0.5f, 0.25f));
            await UniTask.WaitForSeconds(interactionDuration);
            Respawn().Forget();
        }

        private async UniTask Respawn()
        {
            viewRoot.SetActive(false);
            CanInteract = false;
            await UniTask.WaitForSeconds(15);
            _obstacle.enabled = true;
            viewRoot.SetActive(true);
            CanInteract = true;
        }
    }
}