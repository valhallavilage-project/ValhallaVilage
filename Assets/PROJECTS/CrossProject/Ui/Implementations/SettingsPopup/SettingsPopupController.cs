using CrossProject.Ui.Core;

namespace CrossProject.Ui.Implementations.SettingsPopup
{
    public class SettingsPopupController
    {
        private readonly UiService _uiService;

        private SettingsPopup _view;

        public SettingsPopupController(UiService uiService)
        {
            _uiService = uiService;
        }

        public async void OpenSettings()
        {
            var model = new SettingsPopupModel();
            _view = await _uiService.TryOpen(model) as SettingsPopup;
        }
    }
}