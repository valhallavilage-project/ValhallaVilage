using System;
using UnityEngine;

namespace CrossProject.Ui.Core
{
    public interface IUiView
    {
        Type ModelType { get; }
        GameObject AddressablesInstance { get; }
        void BindModel(UiModel model);
        void OnShow();
        void OnClose();
    }
}