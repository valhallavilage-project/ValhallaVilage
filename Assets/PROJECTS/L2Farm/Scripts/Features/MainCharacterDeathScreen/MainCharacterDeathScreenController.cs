using System;
using System.Threading;
using CrossProject.Core;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer.Unity;

namespace L2Farm.Features
{
    public class MainCharacterDeathScreenController : IInitializable, IDisposable
    {
        private readonly UiService _uiService;
        private readonly IMainCharacterReviveGlobalHandler _reviveGlobalHandler;
        private readonly CancellationTokenSource _disposeCts = new();

        private MainCharacterDeathScreen _deathScreen;
        private bool _isOpening;
        private bool _isDeadStateActive;

        public bool IsInitialized { get; private set; }

        public MainCharacterDeathScreenController(UiService uiService, IMainCharacterGlobalFacade mainCharacterFacade, IMainCharacterReviveGlobalHandler reviveGlobalHandler)
        {
            _uiService = uiService;
            _reviveGlobalHandler = reviveGlobalHandler;
            
            mainCharacterFacade.IsDied.WithoutCurrent().ForEachAwaitAsync(MainCharacterDied, _disposeCts.Token).Forget();
            _reviveGlobalHandler.Revived.WithoutCurrent().ForEachAsync(_ => HandleRevive(), _disposeCts.Token).Forget();
        }

        public async UniTask Initialize()
        {
            IsInitialized = true;
        }

        private async UniTask MainCharacterDied(bool isDied)
        {
            // Если получили сигнал о смерти, но мы уже "в состоянии смерти" - игнорируем
            if (isDied && !_isDeadStateActive)
            {
                _isDeadStateActive = true;
                CloseAllActiveScreens();
                await OpenScreen();
            }
        }

        private void HandleRevive()
        {
            _isDeadStateActive = false;
            CloseScreen();
        }

        private void CloseAllActiveScreens()
        {
            try 
            {
                var activeScreen = _uiService.GetFirst<ScreenModel>();
                if (activeScreen != null)
                {
                    // Важно: не закрываем сами себя, если вдруг попали сюда
                    if (activeScreen is not MainCharacterDeathScreenModel)
                    {
                        Debug.Log($"[MainCharacterDeathScreenController] Closing active screen: {activeScreen.GetType().Name}");
                        _uiService.Close(activeScreen);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[MainCharacterDeathScreenController] Error closing screen: {e.Message}");
            }
        }

        private async UniTask OpenScreen()
        {
            if (_deathScreen != null || _isOpening) return;
            
            _isOpening = true;
            try
            {
                var model = new MainCharacterDeathScreenModel()
                {
                    MainCharacterReviveHandler = _reviveGlobalHandler,
                    Close = CloseScreen
                };
                
                _deathScreen = await _uiService.TryOpen(model) as MainCharacterDeathScreen;
            }
            finally
            {
                _isOpening = false;
            }
        }

        private void CloseScreen()
        {
            if (_isOpening)
            {
                CloseScreenAsync().Forget();
                return;
            }

            if (_deathScreen != null)
            {
                _uiService.Close(_deathScreen);
                _deathScreen = null;
            }
        }

        private async UniTaskVoid CloseScreenAsync()
        {
            await UniTask.WaitUntil(() => !_isOpening);
            CloseScreen();
        }

        public void Dispose()
        {
            _disposeCts.Cancel();
            _disposeCts.Dispose();
        }
    }
}