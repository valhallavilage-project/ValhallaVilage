using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrossProject.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CrossProject.Ui.Core
{
    public class UiQueue<TUiModel> : UiRule<TUiModel> where TUiModel : UiModel
    {
        private readonly Queue<UiModel> _queue = new();
        private IUiView _current;
        private bool _isLoading;

        public override event Action<IUiView> OnOpen;
        public override event Action<IUiView> OnClose;
        public override event Action<IUiView> OnHide;
        public override event Action<IUiView> OnReveal;

        public UiQueue(RectTransform root, AddressablesManager addressablesManager) : base(root, addressablesManager) {}

        private async Task<IUiView> OpenInternal(UiModel model)
        {
            _isLoading = true;

            var key = string.IsNullOrEmpty(model.AssetOverride)
                ? model.GetType().Name[..^5]
                : model.AssetOverride;
            var prefab = await GetPrefab(key);
            var instance = Object.Instantiate(prefab, _root);
            instance.name = key;
            _current = instance.GetComponent<IUiView>();

            _isLoading = false;

            _current.BindModel(model);
            _current.OnOpen();
            OnOpen?.Invoke(_current);
            return _current;
        }

        private async UniTask<IUiView> TryOpenFromQueue()
        {
            if (_queue.Count <= 0)
                return null;

            var model = _queue.Dequeue();
            return await OpenInternal(model);
        }

        public override async UniTask<IUiView> Open(UiModel model)
        {
            if (_queue.Count == 0 && _current == null && !_isLoading)
            {
                return await OpenInternal(model);
            }

            _queue.Enqueue(model);
            if (_current == null && _queue.Count > 1)
                await TryOpenFromQueue();

            return null;
        }

        public override void Close(IUiView _)
        {
            OnClose?.Invoke(_current);
            _current?.OnClose();
            _current = null;
            TryOpenFromQueue().Forget();
        }

        public override IUiView Hide(IUiView view)
        {
            OnHide?.Invoke(view);
            view.OnHide();
            return view;
        }

        public override IUiView Reveal(IUiView view)
        {
            OnReveal?.Invoke(view);
            view.OnReveal();
            return view;
        }

        public override IEnumerable<IUiView> Get<TUiModel1>(Func<IUiView, bool> predicate = null)
        {
            return new[] { _current };
        }

        public override IUiView GetFirst<TUiModel1>(Func<IUiView, bool> predicate = null)
        {
            return _current;
        }
    }
}