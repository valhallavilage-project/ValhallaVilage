using System;
using System.Threading;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Core
{
    public interface ISaveRestorableParameterHandler
    {
    }

    public abstract class BaseSaveRestorableParameterHandler<TStatePart> : ISaveRestorableParameterHandler, IInitializable, IDisposable
        where TStatePart : class, IRestorableParameterStatePart, new()
    {
        private readonly ILifetimeParameterHandler _parameterHandler;
        private readonly GameStateManager _gameStateManager;
        private readonly RestorationConfig _restorationConfig;
        private readonly ITimeService _timeService;
        private readonly CancellationTokenSource _disposeCts = new();

        public bool IsInitialized { get; private set; }

        protected CancellationToken DisposeToken => _disposeCts.Token;

        protected BaseSaveRestorableParameterHandler(ILifetimeParameterHandler parameterHandler,
            GameStateManager gameStateManager, RestorationConfig restorationConfig, ITimeService timeService)
        {
            _parameterHandler = parameterHandler;
            _gameStateManager = gameStateManager;
            _restorationConfig = restorationConfig;
            _timeService = timeService;
        }

        public virtual async UniTask Initialize()
        {
            if (!_gameStateManager.State.TryGet<TStatePart>(out var statePart))
            {
                statePart = new TStatePart
                {
                    Value = GetInitialParameterValue(),
                    LastRestoreTime = _timeService.Now
                };

                _gameStateManager.State.Set(statePart);
                _gameStateManager.Save();
            }

            var timesToRestore = (float)(_timeService.Now - statePart.LastRestoreTime).TotalSeconds / _restorationConfig.IntervalInSeconds;
            var amountAfterRestoration = Mathf.Clamp(statePart.Value + timesToRestore * _restorationConfig.ValueToRestoreForOneInterval, 0, GetMaxParameterValue());

            _parameterHandler.Init(GetMaxParameterValue(), amountAfterRestoration, GetMinParameterValue());

            SubscribeOnValueChanged();
            
            IsInitialized = true;
        }

        protected abstract float GetInitialParameterValue();
        protected abstract float GetMaxParameterValue();
        protected abstract float GetMinParameterValue();
        protected abstract void SubscribeOnValueChanged();

        protected void ParameterChanged(float newValue, DateTime lastRestorationTime)
        {
            var statePart = _gameStateManager.State.Get<TStatePart>();

            statePart.Value = newValue;
            statePart.LastRestoreTime = lastRestorationTime;

            _gameStateManager.Save();
        }

        public void Dispose()
        {
            _disposeCts.Cancel();
            _disposeCts.Dispose();
        }
    }
}
