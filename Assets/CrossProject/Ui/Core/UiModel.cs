using System;

namespace CrossProject.Ui.Core
{
    public abstract class UiModel
    {
        public Action OnShowRequest;
        public Action OnShow;
        public Action OnHideRequest;
        public Action OnHide;
    }
}