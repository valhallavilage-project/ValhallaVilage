using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer.Unity;
using Object = UnityEngine.Object;

namespace CrossProject.Core
{
    [DefaultExecutionOrder(-1000)]
    public class AddressablesManager : IPriorityInitializable
    {
        public bool IsInitialized { get; private set; }

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

        public static async UniTask Prewarm()
        {
            await Addressables.LoadAssetsAsync<object>("prewarm", null);
            Debug.Log("Prewarmed");
        }
        public async UniTask Initialize()
        {
            await Prewarm();
            IsInitialized = true;
        }
    }
}