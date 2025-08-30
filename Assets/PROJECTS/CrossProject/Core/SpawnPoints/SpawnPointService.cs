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

        public bool IsInitialized { get; private set; }

        public SpawnPointService(AddressablesManager addressablesManager)
        {
            _addressablesManager = addressablesManager;
        }

        public async UniTask Initialize()
        {
            _spawnPointSetConfig = await _addressablesManager.LoadAssetAsync<SpawnPointSetConfig>();
            IsInitialized = true;
        }

        public Vector3 GetPosition(SpawnPointId id)
        {
            try
            {
                return _spawnPointSetConfig
                    .items
                    .First(x => id == x.id)
                    .position;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to find spawn point with id : {id}; Total Config : {_spawnPointSetConfig.items.Count}");
                throw;
            }
        }

        public Vector3 GetEulerAngles(SpawnPointId id)
        {
            return _spawnPointSetConfig
                .items
                .First(x => id == x.id)
                .eulerAngles;
        }
    }
}
