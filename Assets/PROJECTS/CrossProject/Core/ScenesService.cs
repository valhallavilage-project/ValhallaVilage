using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace CrossProject.Core
{
    public class ScenesService
    {
        public string CurrentSceneName { get; private set; }

        public async UniTask LoadScene(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
        {
            await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
            CurrentSceneName = sceneName;
        }

        public async UniTask UnloadScene(string sceneName)
        {
            await SceneManager.UnloadSceneAsync(sceneName).ToUniTask();
            if (string.Equals(CurrentSceneName, sceneName))
                CurrentSceneName = null;
        }
    }
}