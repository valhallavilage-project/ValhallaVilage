using System.Collections.Generic;
using System.Linq;
using CrossProject.Core;
using CrossProject.Core.Conditions;
using CrossProject.Core.SpawnPoints;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace L2Farm.Features.Buildings
{
    public class BuildingService : IInitializable
    {
        private readonly AddressablesManager _addressablesManager;
        private readonly ConditionService _conditionService;
        private readonly SpawnPointService _spawnPointService;

        private BuildingSetConfig _buildingSetConfig;

        private Dictionary<BuildingId, Building> _buildings = new ();

        public bool IsInitialized { get; private set; }

        public BuildingService(
            AddressablesManager addressablesManager,
            ConditionService conditionService,
            SpawnPointService spawnPointService)
        {
            _addressablesManager = addressablesManager;
            _conditionService = conditionService;
            _spawnPointService = spawnPointService;
        }

        public async UniTask Initialize()
        {
            _buildingSetConfig = await _addressablesManager.LoadAssetAsync<BuildingSetConfig>();

            foreach (var buildingConfig in _buildingSetConfig.items)
                await SpawnBuildingInternal(buildingConfig);

            IsInitialized = true;
        }

        private async UniTask SpawnBuildingInternal(BuildingConfig config)
        {
            var key = _conditionService.Check(config.spawnReadyCondition)
                ? config.assetIdReady
                : config.assetIdBroken;

            var asset = await _addressablesManager.LoadAssetAsync<GameObject>(key);
            var position = _spawnPointService.GetPosition(config.spawnPointId);
            var eulerAngles = _spawnPointService.GetEulerAngles(config.spawnPointId);
            var instance = Object.Instantiate(asset, position, Quaternion.Euler(eulerAngles));
            var building = instance.GetComponent<Building>();
            _buildings[config.id] = building;
            Debug.Log($"[{nameof(BuildingService)}] : spawned {config.id} with asset : {key}!");
        }

        public void StartUpgradeProcess(BuildingId id)
        {
            
        }

        public void SpawnReadyBuilding(BuildingId id)
        {
            var config = _buildingSetConfig.items.FirstOrDefault(x => id == x.id);
            if (config == null)
            {
                Debug.LogError($"[{nameof(BuildingService)}] : there is no config for {id}");
                return;
            }

            if (!_conditionService.Check(config.spawnReadyCondition))
            {
                Debug.LogError($"[{nameof(BuildingService)}] : building {id} do not satisfy it's condition!");
                return;
            }

            if (_buildings.TryGetValue(id, out var building) && !building.IsReady)
                Object.Destroy(building.gameObject);

            SpawnBuildingInternal(config).Forget();
        }
    }
}
