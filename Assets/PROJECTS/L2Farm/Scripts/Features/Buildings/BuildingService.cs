using System;
using System.Collections.Generic;
using System.Linq;
using CrossProject.Core;
using CrossProject.Core.Conditions;
using CrossProject.Core.SaveLoad;
using CrossProject.Core.SpawnPoints;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace L2Farm.Features.Buildings
{
    public class BuildingService : IInitializable
    {
        private readonly AddressablesManager _addressablesManager;
        private readonly ConditionService _conditionService;
        private readonly SpawnPointService _spawnPointService;
        private readonly GameStateManager _gameStateManager;
        private readonly IMainCharacterGlobalExperienceGainHandler _mainCharacterGlobalExperienceGainHandler;
        private readonly ITimerCreator _timerCreator;

        private BuildingSetConfig _buildingSetConfig;

        private Dictionary<BuildingId, Building> _buildings = new();

        public bool IsInitialized { get; private set; }

        public BuildingService(
            AddressablesManager addressablesManager,
            ConditionService conditionService,
            SpawnPointService spawnPointService,
            GameStateManager gameStateManager,
            IMainCharacterGlobalExperienceGainHandler mainCharacterGlobalExperienceGainHandler,
            ITimerCreator timerCreator)
        {
            _addressablesManager = addressablesManager;
            _conditionService = conditionService;
            _spawnPointService = spawnPointService;
            _gameStateManager = gameStateManager;
            _mainCharacterGlobalExperienceGainHandler = mainCharacterGlobalExperienceGainHandler;
            _timerCreator = timerCreator;
        }

        public async UniTask Initialize()
        {
            _buildingSetConfig = await _addressablesManager.LoadAssetAsync<BuildingSetConfig>();

            var part = _gameStateManager.State.Get<BuildingPart>();

            foreach (var buildingConfig in _buildingSetConfig.items)
            {
                if (!part.requests.TryGetValue(buildingConfig.id, out var time))
                    await SpawnBuildingInternal(buildingConfig);
                else
                    await SpawnBuildingInternal(buildingConfig, (int)(DateTime.Now - time).TotalSeconds);
            }

            IsInitialized = true;
        }

        private async UniTask SpawnBuildingInternal(BuildingConfig config, int elapsedSeconds = -1)
        {
            if (_buildings.TryGetValue(config.id, out var buildingInstance))
            {
                if (buildingInstance.IsReady)
                    return;

                Object.Destroy(buildingInstance);
                _buildings.Remove(config.id);
            }

            string key = _conditionService.Check(config.spawnReadyCondition)
                ? config.assetIdReady
                : config.assetIdBroken;

            var asset = await _addressablesManager.LoadAssetAsync<GameObject>(key);
            var position = _spawnPointService.GetPosition(config.spawnPointId);
            var eulerAngles = _spawnPointService.GetEulerAngles(config.spawnPointId);
            var instance = Object.Instantiate(asset, position, Quaternion.Euler(eulerAngles));
            var building = instance.GetComponent<Building>();
            _buildings[config.id] = building;

            if (elapsedSeconds >= 0)
            {
                var timeLeft = Mathf.Clamp(config.timeToBuildInSeconds - elapsedSeconds, 0, config.timeToBuildInSeconds);
                
                _timerCreator.Launch(timeLeft, building.transform.position + config.buildingVFXOffset)
                    .BindBuilding(config.id)
                    .CorrectVfxScale(config.buildingVFXScale)
                    .BindQuest(config.questToLaunchOnComplete)
                    .Start();
            }
        }

        public async UniTask StartUpgradeProcess(BuildingId id)
        {
            Debug.Log($"[{nameof(BuildingService)}] : start upgrade : {id}");
            var part = _gameStateManager.State.Get<BuildingPart>();
            part.requests.Add(id, DateTime.Now);
            _gameStateManager.Save();
            
            var config = _buildingSetConfig.items.First(x => x.id == id);
            var timeLeft = _buildingSetConfig.GetSecondsFor(id);
            
            _timerCreator.Launch(timeLeft, _buildings[id].transform.position + config.buildingVFXOffset)
                .BindBuilding(id)
                .CorrectVfxScale(config.buildingVFXScale)
                .BindQuest(_buildingSetConfig.GetQuestFor(id))
                .BindSoundFx(config.buildingSound)
                .Start();
        }

        public async UniTask SpawnReadyBuilding(BuildingId id)
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

            _mainCharacterGlobalExperienceGainHandler.GainXp(config.completeBuildingXpReward);

            await SpawnBuildingInternal(config);
        }

        public Vector3 GetVFXPositionFor(BuildingId buildingId)
        {
            var offset = _buildingSetConfig.items.First(x => x.id == buildingId).buildingVFXOffset;

            return _buildings[buildingId].transform.position + offset;
        }

        public float GetVFXScaleFor(BuildingId buildingId)
        {
            return _buildingSetConfig.items.First(x => x.id == buildingId).buildingVFXScale;
        }
    }
}
