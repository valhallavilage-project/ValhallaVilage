using System;

namespace CrossProject.Ui.Core
{
    public abstract class UiModel
    {
        public string AssetOverride = null;

        public Action OnOpenRequest;
        public Action OnOpen;
        public Action OnCloseRequest;
        public Action OnClose;
        public Action OnShowRequest;
        public Action OnShow;
        public Action OnHideRequest;
        public Action OnHide;
    }
}