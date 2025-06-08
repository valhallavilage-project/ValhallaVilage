using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CrossProject.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

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
            //TODO : VM : remove recursion
            //_current = await uiService.TryOpen(model) as TUiView;
            _isLoading = false;
            OnOpen?.Invoke(_current);
            return _current;
        }

        private async UniTask<IUiView> TryOpenFromQueue()
        {
            if (_queue.Count <= 0)
                return null;

            return await OpenInternal(_queue.Dequeue());
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
            _current.OnClose();
            //uiService.Close(_current);
            _current = null;
            TryOpenFromQueue().Forget();
        }

        public override IUiView Hide(IUiView _)
        {
            throw new System.NotImplementedException();
        }

        public override IUiView Reveal(IUiView _)
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerable<IUiView> Get<TUiModel1>(Func<IUiView, bool> predicate = null)
        {
            throw new NotImplementedException();
        }

        public override IUiView GetFirst<TUiModel1>(Func<IUiView, bool> predicate = null)
        {
            return _current;
        }
    }
}