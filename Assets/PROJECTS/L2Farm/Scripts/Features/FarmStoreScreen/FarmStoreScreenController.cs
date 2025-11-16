using System;
using System.Threading;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace L2Farm
{
    public class FarmStoreScreenController : IInitializable, IDisposable
    {
        private readonly CancellationTokenSource _disposeCts = new ();
        private readonly UiService _uiService;
        private FarmStoreScreen _screen;

        public bool IsInitialized { get; private set; }

        public FarmStoreScreenController(UiService uiService, IGlobalOpenFarmStoreScreenHandler openScreenHandler)
        {
            _uiService = uiService;
            
            openScreenHandler.ScreenOpenRequested.Listen(OpenScreen, _disposeCts.Token);
        }

        private async UniTask OpenScreen()
        {
            _screen = await _uiService.TryOpen(new FarmStoreScreenModel
            {
                Close = Close
            }) as FarmStoreScreen;
        }

        private void Close()
        {
            _uiService.Close(_screen);
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
