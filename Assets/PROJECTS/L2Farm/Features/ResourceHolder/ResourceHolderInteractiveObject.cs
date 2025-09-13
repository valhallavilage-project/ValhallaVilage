using CrossProject.Core;
using CrossProject.Core.InGameResources;
using CrossProject.Core.Interactions;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace L2Farm.Features.ResourceHolder
{
    public class ResourceHolderInteractiveObject : AbstractInteractiveObject, IResourceData
    {
        [SerializeField] private ResourceContent content;
        [SerializeField] private int energyRequired;
        [SerializeField] private int respawnInSeconds = 60;
        [SerializeField] private AudioSource audio;
        [SerializeField] private NavMeshObstacle obstacle;

        private IMainCharacterSharedDataHolder _mainCharacterSharedData;
        private GameStateManager _gameStateManager;

        public int EnergyRequired => energyRequired;

        public override bool CanInteract() => _mainCharacterSharedData.CurrentEnergy.Value >= energyRequired && viewRoot.activeSelf && !isBusy;

        private void Start()
        {
            Injector.Instance.Inject(this);
        }

        [Inject]
        private void Construct(
            GameStateManager gameStateManager,
            IMainCharacterSharedDataHolder mainCharacterSharedData)
        {
            _gameStateManager = gameStateManager;
            _mainCharacterSharedData = mainCharacterSharedData;
        }

        private async UniTask RespawnTask()
        {
            viewRoot.gameObject.SetActive(false);
            obstacle.enabled = false;
            await UniTask.WaitForSeconds(respawnInSeconds);
            viewRoot.gameObject.SetActive(true);
            obstacle.enabled = true;
            await viewRoot.transform.DOScale(Vector3.one, 1).SetTarget(this);
        }

        protected override async UniTask AfterInteraction()
        {
            var part = _gameStateManager.State.Get<ResourceContentPart>();
            part.Edit(content.Resource, content.Amount);
            _gameStateManager.Save();

            audio.Play();
            DOTween.Kill(this);
            await viewRoot.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.2f);
            await viewRoot.transform.DOScale(Vector3.zero, 0.2f);

            RespawnTask().Forget();
        }

    }
}