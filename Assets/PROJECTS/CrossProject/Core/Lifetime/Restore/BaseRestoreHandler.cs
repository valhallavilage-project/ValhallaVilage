using System;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace CrossProject.Core
{
    public interface IRestoreHandler
    {
        DateTime LastRestoreTime { get; }
    }

    public abstract class BaseRestoreHandler : IRestoreHandler, ITickable, IInitializable
    {
        private readonly RestorationConfig _restorationConfig;
        private readonly ILifetimeParameterHandler _parameterHandler;
        private readonly ITimeService _timeService;

        public bool IsInitialized { get; private set; }
        public DateTime LastRestoreTime { get; private set; }

        protected BaseRestoreHandler(RestorationConfig restorationConfig, ILifetimeParameterHandler parameterHandler, ITimeService timeService)
        {
            _restorationConfig = restorationConfig;
            _parameterHandler = parameterHandler;
            _timeService = timeService;
        }

        public async UniTask Initialize()
        {
            await UniTask.WaitUntil(() => _parameterHandler.IsInitialized);
            
            LastRestoreTime = _timeService.Now;

            IsInitialized = true;
        }

        public void Tick()
        {
            if (!_parameterHandler.IsInitialized)
            {
                return;
            }
            
            if (_parameterHandler.IsFullyRestored)
            {
                LastRestoreTime = _timeService.Now;
            }

            if (_timeService.Now > LastRestoreTime.AddSeconds(_restorationConfig.IntervalInSeconds))
            {
                LastRestoreTime = _timeService.Now;
                _parameterHandler.Restore(_restorationConfig.ValueToRestoreForOneInterval);
            }
        }
    }
}
