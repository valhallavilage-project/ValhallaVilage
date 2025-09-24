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
        private readonly ITimeService _timeService;

        private ProductionSetConfig _productionSetConfig;

        public bool IsInitialized { get; private set; }

        public ProductionService(
            AddressablesManager addressablesManager,
            BuildingService buildingService,
            GameStateManager gameStateManager,
            ITimeService timeService)
        {
            _addressablesManager = addressablesManager;
            _buildingService = buildingService;
            _gameStateManager = gameStateManager;
            _timeService = timeService;
        }

        public async UniTask Initialize()
        {
            await UniTask.WaitUntil(() => _buildingService.IsInitialized);
            _productionSetConfig = await _addressablesManager.LoadAssetAsync<ProductionSetConfig>();
            IsInitialized = true;
        }

        public async UniTask StartProduction(ProductionId productionId)
        {
            await UniTask.WaitUntil(() => IsInitialized);

            Debug.Log($"[{nameof(ProductionService)}] : start production : {productionId}");
            
            var productionConfig = _productionSetConfig.productionConfigs.First(x => x.id == productionId);
            var part = _gameStateManager.State.Get<ProductionPart>();

            if (!part.requests.ContainsKey(productionId))
            {
                part.requests.Add(productionId, _timeService.Now.AddSeconds(productionConfig.timeToProduceInSeconds));
                _gameStateManager.Save();
            }
            
            var timeLeft = (int)Math.Clamp((part.requests[productionId] - _timeService.Now).TotalSeconds, 0, productionConfig.timeToProduceInSeconds);

            var timerPrefab = await _addressablesManager.LoadAssetAsync<GameObject>(nameof(BuildingTimer));
            var timerInstance = Object.Instantiate(timerPrefab, _buildingService.GetVFXPositionFor(productionConfig.buildingId), Quaternion.identity);
            var vfxScale = _buildingService.GetVFXScaleFor(productionConfig.buildingId);
            timerInstance.GetComponent<BuildingTimer>().Setup(timeLeft, (ProductionId)productionConfig.id, productionConfig.finishQuest, vfxScale);
        }
    }
}
