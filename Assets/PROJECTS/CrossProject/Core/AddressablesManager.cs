using System;
using System.Threading;
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
        private const int InitializeTimeoutSec = 15;
        private const int LoadAssetTimeoutSec = 15;
        private const int PrewarmTimeoutSec = 20;

        public bool IsInitialized { get; private set; }

        public async UniTask<T> LoadAssetAsync<T>(string addressableName = null) where T : class
        {
            addressableName ??= typeof(T).Name;

            try
            {
                await Addressables.InitializeAsync().ToUniTask()
                    .Timeout(TimeSpan.FromSeconds(InitializeTimeoutSec));
            }
            catch (TimeoutException)
            {
                Debug.LogError($"[Addressables] InitializeAsync timed out after {InitializeTimeoutSec}s while loading '{addressableName}'. Is the Addressables bundle built and shipped with the APK?");
                throw;
            }

            try
            {
                var asyncOperationHandle = Addressables.LoadAssetAsync<T>(addressableName);
                var result = await asyncOperationHandle.ToUniTask()
                    .Timeout(TimeSpan.FromSeconds(LoadAssetTimeoutSec));
                if (result == null)
                    throw new Exception($"Can't find {addressableName}");
                return result;
            }
            catch (TimeoutException)
            {
                Debug.LogError($"[Addressables] LoadAsset '{addressableName}' timed out after {LoadAssetTimeoutSec}s. Missing from catalog or broken bundle.");
                throw;
            }
        }

        public void ReleaseInstance(GameObject instance)
        {
            Addressables.ReleaseInstance(instance);
            Object.Destroy(instance);
        }

        public static async UniTask Prewarm()
        {
            try
            {
                await Addressables.LoadAssetsAsync<object>("prewarm", null).ToUniTask()
                    .Timeout(TimeSpan.FromSeconds(PrewarmTimeoutSec));
                Debug.Log("Prewarmed");
            }
            catch (TimeoutException)
            {
                Debug.LogError($"[Addressables] Prewarm timed out after {PrewarmTimeoutSec}s. Continuing without prewarm. Check that Addressables content is built.");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Addressables] Prewarm failed: {e.Message}");
            }
        }

        public async UniTask Initialize()
        {
            await Prewarm();
            IsInitialized = true;
        }
    }
}