using UnityEngine;

namespace CrossProject.Ui.Core
{
    public interface IUiView
    {
        GameObject AddressablesInstance { get; }
        void BindModel(UiModel model);
        void OnShow();
        void OnClose();
    }
}