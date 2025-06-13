using System;
using System.Collections.Generic;
using System.Linq;
using CrossProject.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CrossProject.Ui.Core
{
    public class UiDictionary<TUiModel> : UiRule<TUiModel> where TUiModel : UiModel
    {
        private readonly Dictionary<Type, List<IUiView>> _dictionary = new();

        public override event Action<IUiView> OnOpen;
        public override event Action<IUiView> OnClose;
        public override event Action<IUiView> OnHide;
        public override event Action<IUiView> OnReveal;

        public UiDictionary(RectTransform root, AddressablesManager addressablesManager) : base(root, addressablesManager) {}

        public override async UniTask<IUiView> Open(UiModel model)
        {
            var key = string.IsNullOrEmpty(model.AssetOverride)
                ? model.GetType().Name[..^5]
                : model.AssetOverride;
            var prefab = await GetPrefab(key);
            var instance = Object.Instantiate(prefab, _root);
            instance.name = key;

            var view = instance.GetComponent<IUiView>();
            view.BindModel(model);
            var type = view.GetType();

            if (_dictionary.ContainsKey(type))
                _dictionary[type].Add(view);
            else
                _dictionary.Add(type, new List<IUiView>{ view });
            return view;
        }

        public override void Close(IUiView view)
        {
            OnClose?.Invoke(view);
            view.OnClose();
        }

        public override IUiView Hide(IUiView view)
        {
            OnHide?.Invoke(view);
            return view;
        }

        public override IUiView Reveal(IUiView view)
        {
            OnReveal?.Invoke(view);
            return view;
        }

        public override IEnumerable<IUiView> Get<TUiModel1>(Func<IUiView, bool> predicate = null)
        {
            var type = typeof(TUiModel1);
            if (!_dictionary.TryGetValue(type, out var views))
                return null;

            return predicate == null
                ? views
                : views.Where(predicate);
        }

        public override IUiView GetFirst<TUiModel1>(Func<IUiView, bool> predicate = null)
        {
            var type = typeof(TUiModel1);
            if (!_dictionary.TryGetValue(type, out var views))
                return null;

            if (predicate == null)
                return views.FirstOrDefault();

            foreach (var view in views)
                if (predicate(view))
                    return view;

            return null;
        }
    }
}