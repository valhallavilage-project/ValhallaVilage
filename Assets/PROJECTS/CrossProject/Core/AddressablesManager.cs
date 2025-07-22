using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace CrossProject.Core
{
    public class AddressablesManager
    {
        public async UniTask<T> LoadAssetAsync<T>(string addressableName = null) where T : class
        {
            await Addressables.InitializeAsync();
            addressableName ??= typeof(T).Name;
            var asyncOperationHandle = Addressables.LoadAssetAsync<T>(addressableName);
            var result = await asyncOperationHandle;
            if (result == null)
                throw new Exception($"Can't find {addressableName}");
            return result;
        }

        public void ReleaseInstance(GameObject instance)
        {
            Addressables.ReleaseInstance(instance);
            Object.Destroy(instance);
        }
    }
}