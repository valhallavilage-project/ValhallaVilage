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
            return _spawnPointSetConfig
                .items
                .First(x => id == new SpawnPointId(x.id))
                .position;
        }
    }
}
