using System;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace CrossProject.Core
{
    public interface IRestoreEnergyHandler
    {
        DateTime LastRestoreTime { get; }
    }

    public class RestoreEnergyHandler : IRestoreEnergyHandler, IInitializable, ITickable
    {
        private readonly EnergyRestorationConfig _energyRestorationConfig;
        private readonly IEnergyHandler _energyHandler;
        private readonly ITimeService _timeService;

        public bool IsInitialized { get; private set; }
        public DateTime LastRestoreTime { get; private set; }

        public RestoreEnergyHandler(EnergyRestorationConfig energyRestorationConfig, IEnergyHandler energyHandler, ITimeService timeService)
        {
            _energyRestorationConfig = energyRestorationConfig;
            _energyHandler = energyHandler;
            _timeService = timeService;
        }

        public async UniTask Initialize()
        {
            await UniTask.WaitUntil(() => _energyHandler.IsInitialized);
            
            LastRestoreTime = _timeService.Now;

            IsInitialized = true;
        }

        public void Tick()
        {
            if (!_energyHandler.IsInitialized)
            {
                return;
            }
            
            if (_energyHandler.IsFullyRestored)
            {
                LastRestoreTime = _timeService.Now;
            }

            if (_timeService.Now > LastRestoreTime.AddSeconds(_energyRestorationConfig.IntervalInSeconds))
            {
                LastRestoreTime = _timeService.Now;
                _energyHandler.Restore(_energyRestorationConfig.EnergyToRestoreForOneInterval);
            }
        }
    }
}
