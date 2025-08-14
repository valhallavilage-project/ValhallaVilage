using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer.Unity;

namespace CrossProject.Core
{
    public class ScenesService
    {
        private LifetimeScope _parent;
        public string CurrentSceneName { get; private set; }

        public ScenesService(LifetimeScope scope)
        {
            _parent = scope;
        }

        public async UniTask LoadScene(string sceneName)
        {
            using (LifetimeScope.EnqueueParent(_parent))
            {
                await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive).ToUniTask();
                CurrentSceneName = sceneName;
            }
        }

        public async UniTask UnloadScene(string sceneName)
        {
            Debug.LogWarning(sceneName + " Unload");
            await SceneManager.UnloadSceneAsync(sceneName);
            if (string.Equals(CurrentSceneName, sceneName))
                CurrentSceneName = null;
        }
    }
}