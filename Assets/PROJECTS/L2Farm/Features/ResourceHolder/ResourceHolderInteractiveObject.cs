using CrossProject.Core;
using CrossProject.Core.Energy;
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
    public class ResourceHolderInteractiveObject : AbstractInteractiveObject
    {
        [SerializeField] private ResourceContent content;
        [SerializeField] private int energyRequired;
        [SerializeField] private int respawnInSeconds = 60;
        [SerializeField] private AudioSource audio;
        [SerializeField] private NavMeshObstacle obstacle;

        private IEnergyProvider _energyProvider;
        private GameStateManager _gameStateManager;

        public override bool CanInteract() => _energyProvider.CurrentValue >= energyRequired && viewRoot.activeSelf;

        private void Start()
        {
            ManualPrefabInjector.Instance.Inject(this);
        }

        [Inject]
        private void Construct(
            GameStateManager gameStateManager,
            IEnergyProvider energyProvider)
        {
            _gameStateManager = gameStateManager;
            _energyProvider = energyProvider;
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
            await viewRoot.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.1f);
            await viewRoot.transform.DOScale(Vector3.zero, 0.1f);

            RespawnTask().Forget();
        }
    }
}