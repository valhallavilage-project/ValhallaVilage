using System;
using System.Threading;
using CrossProject.Core;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using VContainer.Unity;

namespace L2Farm.Features
{
    public class MainCharacterDeathScreenController : IInitializable, IDisposable
    {
        private readonly UiService _uiService;
        private readonly IMainCharacterReviveGlobalHandler _reviveGlobalHandler;
        private readonly CancellationTokenSource _disposeCts = new();

        private MainCharacterDeathScreen _deathScreen;

        public bool IsInitialized { get; private set; }

        public MainCharacterDeathScreenController(UiService uiService, IMainCharacterGlobalFacade mainCharacterFacade, IMainCharacterReviveGlobalHandler reviveGlobalHandler)
        {
            _uiService = uiService;
            _reviveGlobalHandler = reviveGlobalHandler;
            mainCharacterFacade.IsDied.WithoutCurrent().ForEachAwaitAsync(MainCharacterDied, _disposeCts.Token).Forget();
        }

        public async UniTask Initialize()
        {
            IsInitialized = true;
        }

        private async UniTask MainCharacterDied(bool _)
        {
            await OpenScreen();
        }

        private async UniTask OpenScreen()
        {
            _deathScreen = await _uiService.TryOpen(new MainCharacterDeathScreenModel()
            {
                Close = () => _uiService.Close(_deathScreen),
                MainCharacterReviveHandler = _reviveGlobalHandler
            }) as MainCharacterDeathScreen;
        }

        public void Dispose()
        {
            _disposeCts.Cancel();
            _disposeCts.Dispose();
        }
    }
}
