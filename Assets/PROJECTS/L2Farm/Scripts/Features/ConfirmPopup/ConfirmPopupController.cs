using System;
using System.Threading;
using CrossProject.Ui.Core;
using CrossProject.Ui.Implementations;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using VContainer.Unity;

namespace L2Farm.Features
{
    public class ConfirmPopupController : IInitializable, IDisposable
    {
        private ConfirmPopup _popup;
        private readonly UiService _uiService;
        private readonly CancellationTokenSource _disposeCts = new();

        public bool IsInitialized { get; private set; }
        
        public ConfirmPopupController(UiService uiService, IConfirmPopupOpenHandler confirmPopupOpenHandler)
        {
            _uiService = uiService;
            
            confirmPopupOpenHandler.Opened.WithoutCurrent().ForEachAwaitAsync(OpenScreen, _disposeCts.Token).Forget();
        }

        public async UniTask Initialize()
        {
            IsInitialized = true;
        }

        private async UniTask OpenScreen(string text)
        {
            _popup = await _uiService.TryOpen(new ConfirmPopupModel
            {
                Close = () => _uiService.Close(_popup),
                Text = text
            }) as ConfirmPopup;
        }

        public void Dispose()
        {
            _disposeCts.Cancel();
            _disposeCts?.Dispose();
        }
    }
}
