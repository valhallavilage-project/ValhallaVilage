using System;
using System.Threading;
using CrossProject.Core;
using CrossProject.Core.InGameResources;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using VContainer.Unity;

namespace L2Farm
{
    public interface ITimePotionConsumeHandler
    {
    }

    public class TimePotionConsumeHandler : ITimePotionConsumeHandler, IInitializable
    {
        private PotionsConfig _potionsConfig;
        
        private readonly AddressablesManager _addressablesManager;
        private readonly ResourcesService _resourcesService;
        private readonly ITimersHandler _timersHandler;
        private readonly CancellationTokenSource _disposeCts = new();
        public bool IsInitialized { get; private set; }
        
        public TimePotionConsumeHandler(IMainCharacterGlobalPotionConsumeHandler mainCharacterGlobalPotionConsumeHandler,
            AddressablesManager addressablesManager, ResourcesService resourcesService, ITimersHandler timersHandler)
        {
            _addressablesManager = addressablesManager;
            _resourcesService = resourcesService;
            _timersHandler = timersHandler;

            mainCharacterGlobalPotionConsumeHandler.PotionConsumed.WithoutCurrent().ForEachAsync(PotionConsumed, _disposeCts.Token).Forget();
        }

        public async UniTask Initialize()
        {
            _potionsConfig = await _addressablesManager.LoadAssetAsync<PotionsConfig>(nameof(PotionsConfig));

            IsInitialized = true;
        }

        private void PotionConsumed(PotionType potion)
        {
            if (potion != PotionType.Time)
            {
                return;
            }

            var time = (float)_potionsConfig.GetPotion(PotionType.Time).Value;

            while (time > 0)
            {
                var timeLeft = _timersHandler.ReduceTimerSeconds(time);

                if (Math.Abs(timeLeft - time) < float.Epsilon)
                {
                    break;
                }

                time = timeLeft;
            }
            
            _resourcesService.DecreaseResourceValue(new ResourceId("Resource_TimePotion"));
        }

        public void Dispose()
        {
            _disposeCts?.Dispose();
            _disposeCts?.Cancel();
        }
    }
}
