using System;
using System.Collections.Generic;
using System.Linq;
using CrossProject.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Ui.Core
{
    public class UiDictionary<TUiModel, TUiView> : UiRule<TUiModel, TUiView> where TUiModel : UiModel where TUiView : MonoBehaviour, IUiView
    {
        private readonly Dictionary<Type, List<IUiView>> _dictionary = new();

        public override event Action<IUiView> OnOpen;
        public override event Action<IUiView> OnClose;
        public override event Action<IUiView> OnHide;
        public override event Action<IUiView> OnReveal;

        public UiDictionary(RectTransform root, AddressablesManager addressablesManager) : base(root, addressablesManager) {}

        public override async UniTask<IUiView> Open(UiModel model)
        {
            //TODO : Remove recursion
            // var view = await uiService.TryOpen(model) as TUiView;
            // var type = view.GetComponent<IUiView>().GetType();
            // if (_dictionary.ContainsKey(type))
            //     _dictionary[type].Add(view);
            // else
            //     _dictionary.Add(type, new List<IUiView>{ view });
            // return view;
            return null;
        }

        public override void Close(IUiView view)
        {
            OnClose?.Invoke(view);
            view.OnClose();
        }

        public override IUiView Hide(IUiView view)
        {
            throw new NotImplementedException();
        }

        public override IUiView Reveal(IUiView view)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<IUiView> Get<TUiModel1>(Func<IUiView, bool> predicate = null)
        {
            throw new NotImplementedException();
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