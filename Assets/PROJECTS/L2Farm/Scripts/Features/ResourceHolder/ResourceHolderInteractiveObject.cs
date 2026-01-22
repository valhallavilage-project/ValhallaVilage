using CrossProject.Core;
using CrossProject.Core.InGameResources;
using CrossProject.Core.Interactions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using VContainer;

namespace L2Farm.Features.ResourceHolder
{
    public class ResourceHolderInteractiveObject : AbstractInteractiveObject, IResourceData, IExperienceData
    {
        [SerializeField] private ResourceContent content;
        [SerializeField] private int energyRequired;
        [SerializeField] private int respawnInSeconds = 60;
        [SerializeField] private NavMeshObstacle obstacle;
        [SerializeField] private float _experienceReward = 5;

        private IMainCharacterGlobalFacade _mainCharacterSharedData;
        private IResourcesService _resourcesService;

        public int EnergyRequired => energyRequired;
        public float PerformedTaskExperience => _experienceReward;

        public override bool CanInteract() => _mainCharacterSharedData.CurrentEnergy.Value >= energyRequired && viewRoot.activeSelf && !isBusy;

        private void Start()
        {
            Injector.Instance.Inject(this);
        }

        [Inject]
        private void Construct(
            IResourcesService resourcesService,
            IMainCharacterGlobalFacade mainCharacterSharedData)
        {
            _resourcesService = resourcesService;
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
            // Fix: Use ChangeResource to ADD resources, not SetNewResourceValue
            // SetNewResourceValue was causing negative numbers bug
            _resourcesService.ChangeResource(content.Resource, content.Amount);

            DOTween.Kill(this);
            await viewRoot.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.2f);
            await viewRoot.transform.DOScale(Vector3.zero, 0.2f);

            RespawnTask().Forget();
        }

    }
}