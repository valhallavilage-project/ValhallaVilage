using System.Collections.Generic;
using System.Linq;
using CrossProject.Core.Content;
using CrossProject.Core.Energy;
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
        [SerializeReference]
        private IResourceContent _resourceContent;

        [SerializeField]
        private NavMeshObstacle _obstacle;

        [SerializeField]
        private AudioSource _interactSound;

        private GameStateManager _gameStateManager;
        private IEnergyProvider _energyProvider;

        [Inject]
        private void Construct(
            GameStateManager gameStateManager,
            IEnergyProvider energyProvider)
        {
            _gameStateManager = gameStateManager;
            _energyProvider = energyProvider;
        }

        private void Start()
        {
            ManualPrefabInjector.Instance.Inject(this);
        }

        public override async UniTask Interaction()
        {
            if (_energyProvider.CurrentValue < energyNeeded)
                return;

            _obstacle.enabled = false;
            _interactSound.Play();
            //TODO : VM : add player animation
            _energyProvider.Spend(energyNeeded);
            await transform.DOShakeScale(0.2f, new Vector3(0.25f, 0.5f, 0.25f));
            await UniTask.WaitForSeconds(interactionDuration);
            if (_gameStateManager.State.TryGet(out ResourceContentPart part))
            {
                var resource = part.Resources.FirstOrDefault(x => x.Resource == _resourceContent.Resource);
                resource ??= new ResourceContent(_resourceContent.Resource);
                resource.Amount += _resourceContent.Amount;
            }
            else
            {
                part = new ResourceContentPart();
                part.Resources = new List<ResourceContent>
                {
                    new (_resourceContent.Resource, _resourceContent.Amount)
                };
            }
            _gameStateManager.State.Set(part);
            _gameStateManager.Save();
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