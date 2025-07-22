using System;
using CrossProject.Ui.Core;

namespace CrossProject.Ui.Implementations.SettingsPopup
{
    public class SettingsPopupModel : PopupModel
    {
        public Action<float> SetZoom;
        public Action ToggleBGM;
        public Action ToggleSFX;
    }
}
