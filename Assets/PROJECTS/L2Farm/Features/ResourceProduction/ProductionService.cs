using System;
using System.Linq;
using CrossProject.Core;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using L2Farm.Features.Buildings;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace L2Farm.Features.ResourceProduction
{
    public class ProductionService : IInitializable
    {
        private readonly AddressablesManager _addressablesManager;
        private readonly BuildingService _buildingService;
        private readonly GameStateManager _gameStateManager;

        private ProductionSetConfig _productionSetConfig;

        public bool IsInitialized { get; private set; }

        public ProductionService(
            AddressablesManager addressablesManager,
            BuildingService buildingService,
            GameStateManager gameStateManager)
        {
            _addressablesManager = addressablesManager;
            _buildingService = buildingService;
            _gameStateManager = gameStateManager;
        }

        public async UniTask Initialize()
        {
            await UniTask.WaitUntil(() => _buildingService.IsInitialized);
            _productionSetConfig = await _addressablesManager.LoadAssetAsync<ProductionSetConfig>();
            IsInitialized = true;
        }

        public async UniTask StartProduction(ProductionId productionId)
        {
            Debug.Log($"[{nameof(ProductionService)}] : start production : {productionId}");
            var part = _gameStateManager.State.Get<ProductionPart>();
            part.requests.Add(productionId, DateTime.Now);
            _gameStateManager.Save();

            var productionConfig = _productionSetConfig.productionConfigs.First(x => x.id == productionId);
            var timerPrefab = await _addressablesManager.LoadAssetAsync<GameObject>(nameof(BuildingTimer));
            var timerInstance = Object.Instantiate(timerPrefab, _buildingService.GetVFXPositionFor(productionConfig.buildingId), Quaternion.identity);
            var vfxScale = _buildingService.GetVFXScaleFor(productionConfig.buildingId);
            timerInstance.GetComponent<BuildingTimer>().Setup(productionConfig.timeToProduceInSeconds, (ProductionId)productionConfig.id, productionConfig.finishQuest, vfxScale);
        }
    }
}
