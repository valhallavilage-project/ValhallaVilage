using System.Collections.Generic;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using UnityEngine.Assertions;

namespace CrossProject.Ui.Implementations
{
    public class LoadingScreenController
    {
        private readonly UiService _uiService;

        private LoadingScreen _view;

        public LoadingScreenController(UiService uiService)
        {
            _uiService = uiService;
        }

        private LoadingScreenModel GetModel()
        {
            return new LoadingScreenModel
            {
                //TODO : VM : create ModelFactory, that will do this automatically
                //TODO : VM : consider next API -> _uiService.Close(UiModel model)
                Close = () => _uiService.Close(_view),
                //TODO : VM : config for assets overrides per project
                AssetOverride = "L2Farm_LoadingScreen"
            };
        }

        public async void Load(IReadOnlyList<UniTask> tasks)
        {
            _view = await _uiService.TryOpen(GetModel()) as LoadingScreen;
            Assert.IsNotNull(_view);

            if (tasks.Count > 0)
            {
                for (int i = 0; i < tasks.Count; i++)
                {
                    _view.UpdateProgress(i / (float)tasks.Count).Forget();
                    await tasks[i];
                }
                await _view.UpdateProgress(1);
            }

            _uiService.Close(_view);
        }
    }
}