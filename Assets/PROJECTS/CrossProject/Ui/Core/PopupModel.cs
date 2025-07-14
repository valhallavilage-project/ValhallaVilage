using System;

namespace CrossProject.Ui.Core
{
    public abstract class PopupModel : UiModel
    {
        public Action Close { get; set; }
        public Action Accept { get; set; }
        public Action Cancel { get; set; }
    }
}