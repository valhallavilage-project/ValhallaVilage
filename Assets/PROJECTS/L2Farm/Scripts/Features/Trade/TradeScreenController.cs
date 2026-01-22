using System;
using System.Threading;
using CrossProject.Core;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace L2Farm
{
    public class TradeScreenController : IInitializable, IDisposable
    {
        private readonly UiService _uiService;
        private readonly GameStateManager _gameStateManager;
        private readonly ITimeService _timeService;
        private readonly CancellationTokenSource _disposeCts = new ();

        private TradeScreen _tradeScreen;

        public bool IsInitialized { get; private set; }

        public TradeScreenController(UiService uiService, IGlobalOpenTradeScreenHandler openTradeScreenHandler,
            GameStateManager gameStateManager, ITimeService timeService)
        {
            _uiService = uiService;
            _gameStateManager = gameStateManager;
            _timeService = timeService;

            openTradeScreenHandler.ScreenOpenRequested.Listen(OpenScreen, _disposeCts.Token);
        }

        private async UniTask OpenScreen()
        {
            _tradeScreen = await _uiService.TryOpen(new TradeScreenModel
            {
                Close = Close,
                GameStateManager = _gameStateManager,
                TimeService = _timeService
            }) as TradeScreen;
        }

        private void Close()
        {
            if (_tradeScreen == null)
                return;

            _uiService.Close(_tradeScreen);
            _tradeScreen = null;
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
