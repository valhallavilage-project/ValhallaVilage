using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Core.SpawnPoints
{
    public class SpawnPointService : IInitializable
    {
        private readonly AddressablesManager _addressablesManager;

        private SpawnPointSetConfig _spawnPointSetConfig;

        public bool IsInitialized { get; private set; } = false;

        public SpawnPointService(AddressablesManager addressablesManager)
        {
            _addressablesManager = addressablesManager;
        }

        public async UniTask Initialize()
        {
            _spawnPointSetConfig = await _addressablesManager.LoadAssetAsync<SpawnPointSetConfig>();
            Debug.Log($"[{nameof(SpawnPointService)}] : loaded config : {_spawnPointSetConfig?.items?.Count}");
            await UniTask.WaitUntil(() => _spawnPointSetConfig != null);
            IsInitialized = true;
        }

        public Vector3 GetPosition(SpawnPointId id)
        {
            var spawnPoint = _spawnPointSetConfig.items.FirstOrDefault(x => id == x.id);
            if (spawnPoint == null)
            {
                Debug.LogError($"[SpawnPointService] Spawn point not found: {id}; Total configs: {_spawnPointSetConfig.items.Count}");
                return Vector3.zero;
            }

            return spawnPoint.position;
        }

        public Vector3 GetEulerAngles(SpawnPointId id)
        {
            var spawnPoint = _spawnPointSetConfig.items.FirstOrDefault(x => id == x.id);
            if (spawnPoint == null)
            {
                Debug.LogError($"[SpawnPointService] Spawn point not found for angles: {id}");
                return Vector3.zero;
            }

            return spawnPoint.eulerAngles;
        }
    }
}
