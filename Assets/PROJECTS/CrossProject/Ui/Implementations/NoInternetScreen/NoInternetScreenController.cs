using System;
using System.Threading;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Ui.Implementations
{
    public class NoInternetScreenController : IInitializable, IDisposable
    {
        private readonly UiService _uiService;

        private IUiView _view;
        private bool _hasInternet = true;
        private readonly CancellationTokenSource _cancellationTokenSource = new ();
        private readonly TimeSpan _delay = TimeSpan.FromSeconds(5);

        public bool IsInitialized { get; private set; }

        public NoInternetScreenController(UiService uiService)
        {
            _uiService = uiService;
        }

        public async UniTask Initialize()
        {
            Routine(_cancellationTokenSource.Token).Forget();
            IsInitialized = true;
        }

        private async UniTask Routine(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                if (_hasInternet && Application.internetReachability == NetworkReachability.NotReachable)
                {
                    _hasInternet = false;
                    _view = await _uiService.TryOpen(new NoInternetScreenModel());
                    //TODO : VM : Request pause
                }

                await UniTask.Delay(_delay);

                if (!_hasInternet && Application.internetReachability != NetworkReachability.NotReachable)
                {
                    _hasInternet = true;
                    _uiService.Close(_view);
                }
            }
            // ReSharper disable once FunctionNeverReturns
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}