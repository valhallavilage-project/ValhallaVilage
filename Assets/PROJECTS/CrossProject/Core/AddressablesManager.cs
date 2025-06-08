using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CrossProject.Core
{
    public class AddressablesManager
    {
        private readonly Dictionary<string, AsyncOperationHandle> _map = new();

        public async UniTask<T> LoadAssetAsync<T>(string addressableName) where T : class
        {
            await Addressables.InitializeAsync();
            var asyncOperationHandle = Addressables.LoadAssetAsync<T>(addressableName);
            var preloaded = _map.ContainsKey(addressableName);
            if (!preloaded)
                _map.Add(addressableName, asyncOperationHandle);
            else
                return _map[addressableName].Result as T;
            return await asyncOperationHandle;
        }

        public bool ReleaseInstance(GameObject instance)
        {
            var key = instance.name;
            var hasHandle = _map.TryGetValue(key, out var handle);
            if (!hasHandle)
                return false;

            var released = Addressables.ReleaseInstance(handle);
            if (released)
                _map.Remove(key); 

            if (released && instance != null)
                Object.Destroy(instance);

            return released;
        }
    }
}