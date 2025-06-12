using System;
using UnityEngine;

namespace CrossProject.Ui.Core
{
    public abstract class UiView<TUiModel> : MonoBehaviour, IUiView where TUiModel : UiModel
    {
        protected CanvasGroup canvasGroup;

        public TUiModel Model { get; protected set; }
        public Type ModelType => Model.GetType();
        public GameObject AddressablesInstance => gameObject;

        private void Awake()
        {
            TryGetComponent(out canvasGroup);
        }

        public void BindModel(UiModel model)
        {
            if (model is not TUiModel uiModel)
                throw new Exception($"Wrong model type! Expected : {typeof(TUiModel).Name}; Got : {model.GetType().Name}");

            Model = uiModel;
            OnBind();
        }

        protected virtual void OnBind() { }
        public virtual void OnOpen() { }
        public virtual void OnClose() { }

        public virtual void OnHide()
        {
            if (canvasGroup != null)
                canvasGroup.alpha = 0;
        }

        public virtual void OnReveal()
        {
            if (canvasGroup != null)
                canvasGroup.alpha = 1;
        }
    }
}