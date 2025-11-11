using System;
using System.Threading;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace L2Farm
{
    public class TradeScreenController : IInitializable, IDisposable
    {
        private readonly UiService _uiService;
        private readonly CancellationTokenSource _disposeCts = new ();

        private TradeScreen _tradeScreen;

        public bool IsInitialized { get; private set; }

        public TradeScreenController(UiService uiService, IGlobalOpenTradeScreenHandler openTradeScreenHandler)
        {
            _uiService = uiService;
            
            openTradeScreenHandler.ScreenOpenRequested.Listen(OpenScreen, _disposeCts.Token);
        }

        private async UniTask OpenScreen()
        {
            _tradeScreen = await _uiService.TryOpen(new TradeScreenModel
            {
                Close = Close
            }) as TradeScreen;
        }

        private void Close()
        {
            _tradeScreen.Clear();
            
            _uiService.Close(_tradeScreen);
        }

        public async UniTask Initialize()
        {
            IsInitialized = true;
        }

        public void Dispose()
        {
            _disposeCts.Cancel();
            _disposeCts.Dispose();
        }
    }
}
