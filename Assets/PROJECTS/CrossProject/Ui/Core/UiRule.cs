using System;
using System.Collections.Generic;
using CrossProject.Core;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using UnityEngine;

namespace CrossProject.Ui.Core
{
    public abstract class UiRule<TUiModel> : IUiRule where TUiModel : UiModel
    {
        protected readonly RectTransform _root;
        protected readonly AddressablesManager _addressablesManager;

        public abstract event Action<IUiView> OnOpen;
        public abstract event Action<IUiView> OnClose;
        public abstract event Action<IUiView> OnHide;
        public abstract event Action<IUiView> OnReveal;

        public UiRule(
            RectTransform root,
            AddressablesManager addressablesManager)
        {
            _root = root;
            _addressablesManager = addressablesManager;

            var type = typeof(TUiModel);
            if (!type.IsAbstract)
                throw new Exception("Create rules for abstract only models!");
        }

        public bool CanApply(Type modelType) => modelType.InheritsFrom(typeof(TUiModel));

        protected async UniTask<GameObject> GetPrefab(string key)
        {
            return await _addressablesManager.LoadAssetAsync<GameObject>(key);
        }

        public abstract UniTask<IUiView> Open(UiModel model);
        public abstract void Close(IUiView view);
        public abstract IUiView Hide(IUiView view);
        public abstract IUiView Reveal(IUiView view);

        public abstract IEnumerable<IUiView> Get<TUiModel1>(Func<IUiView, bool> predicate = null) where TUiModel1 : UiModel;
        public abstract IUiView GetFirst<TUiModel1>(Func<IUiView, bool> predicate = null) where TUiModel1 : UiModel;
    }
}