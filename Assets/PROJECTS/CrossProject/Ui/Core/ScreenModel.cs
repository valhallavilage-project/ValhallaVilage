using System;

namespace CrossProject.Ui.Core
{
    public abstract class ScreenModel : UiModel
    {
        public Action Close { get; set; }
    }
}