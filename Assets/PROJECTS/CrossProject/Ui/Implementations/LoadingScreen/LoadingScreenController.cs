using CrossProject.Ui.Core;
using VContainer.Unity;

namespace CrossProject.Ui.Implementations
{
    public class LoadingScreenController : IInitializable
    {
        private readonly UiService _uiService;

        private LoadingScreen _view;

        public LoadingScreenController(UiService uiService)
        {
            _uiService = uiService;
        }

        public async void Open()
        {
            var model = new LoadingScreenModel
            {
                //TODO : VM : create ModelFactory, that will do this automatically
                //TODO : VM : consider next API -> _uiService.Close(UiModel model)
                Close = () => _uiService.Close(_view),
                AssetOverride = "L2Farm_LoadingScreen"
            };
            
            _view = await _uiService.TryOpen(model) as LoadingScreen;
        }

        public void Initialize()
        {
            Open();
        }
    }
}