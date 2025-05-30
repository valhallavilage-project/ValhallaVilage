using System;
using UnityEngine;

namespace CrossProject.Ui.Core
{
    public abstract class UiView<TUiModel> : MonoBehaviour, IUiView where TUiModel : UiModel
    {
        public TUiModel Model { get; protected set; }
        public GameObject AddressablesInstance => gameObject;

        public void BindModel(UiModel model)
        {
            if (model is not TUiModel uiModel)
                throw new Exception($"Wrong model type! Expected : {typeof(TUiModel).Name}; Got : {model.GetType().Name}");

            Model = uiModel;
            OnBind();
        }

        protected virtual void OnBind() { }
        public virtual void OnShow() { }
        public virtual void OnClose() { }
    }
}