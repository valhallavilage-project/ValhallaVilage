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
        private readonly UiService _uiService;
        private readonly CancellationTokenSource _disposeCts = new();
        private readonly IConfirmPopupOpenHandler _confirmPopupOpenHandler;
        
        private ConfirmPopup _popup;
        private ConfirmPopupModel _model;

        public bool IsInitialized { get; private set; }

        public ConfirmPopupController(UiService uiService, IConfirmPopupOpenHandler confirmPopupOpenHandler)
        {
            _confirmPopupOpenHandler = confirmPopupOpenHandler;
            _uiService = uiService;

            confirmPopupOpenHandler.Opened.WithoutCurrent().ForEachAwaitAsync(OpenScreen, _disposeCts.Token).Forget();
        }

        public async UniTask Initialize()
        {
            IsInitialized = true;
            
            _model = new ConfirmPopupModel
            {
                Close = () => _uiService.Close(_popup),
            };
            
            _model.Result.WithoutCurrent().ForEachAsync(PopupResultSet, _disposeCts.Token).Forget();
        }

        private async UniTask OpenScreen(ConfirmPopupData data)
        {
            _model.Data = data;
            
            _popup = await _uiService.TryOpen(_model) as ConfirmPopup;
        }

        private void PopupResultSet(bool result)
        {
            _confirmPopupOpenHandler.SetPopupResult(result);
        }

        public void Dispose()
        {
            _disposeCts.Cancel();
            _disposeCts?.Dispose();
        }
    }
}
