using System;
using System.Threading;
using CrossProject.Core.InGameResources;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Core
{
    public interface IMainCharacterGlobalParameterChangesHandler
    {
    }

    public class MainCharacterGlobalParameterChangesHandler : IMainCharacterGlobalParameterChangesHandler, IInitializable, IDisposable
    {
        private readonly IHealthHandler _healthHandler;
        private readonly IEnergyHandler _energyHandler;
        private readonly PotionsConfig _potionsConfig;
        private readonly ResourcesService _resourcesService;
        private readonly GardenConfig _gardenConfig;
        private readonly SimpleMovement.SimpleMovementController _movementController;
        private readonly CancellationTokenSource _disposeCts = new();

        public bool IsInitialized { get; private set; }

        public MainCharacterGlobalParameterChangesHandler(
            IMainCharacterReviveGlobalHandler globalReviveHandler, IReviveAbility reviveAbility,
            IMainCharacterGlobalExperienceGainHandler globalExperienceGainHandler, IExperienceHandler experienceHandler,
            IMainCharacterGlobalPotionConsumeHandler globalPotionConsumeHandler,
            IMainCharacterGlobalCleanGardenBedHandler globalCleanGardenBedHandler, IHealthHandler healthHandler,
            IEnergyHandler energyHandler, PotionsConfig potionsConfig, ResourcesService resourcesService,
            GardenConfig gardenConfig, SimpleMovement.SimpleMovementController movementController)
        {
            _healthHandler = healthHandler;
            _energyHandler = energyHandler;
            _potionsConfig = potionsConfig;
            _resourcesService = resourcesService;
            _gardenConfig = gardenConfig;
            _movementController = movementController;

            globalExperienceGainHandler.ExperienceGained.WithoutCurrent().ForEachAsync(xp => experienceHandler.GainXp(xp), _disposeCts.Token).Forget();
            
            globalReviveHandler.Revived.WithoutCurrent().ForEachAsync(_ =>
            {
                if (globalReviveHandler.RevivePoint != null)
                {
                    Vector3 pos = globalReviveHandler.RevivePoint.position;
                    
                    // Сначала лечим
                    reviveAbility.Revive(pos);
                    
                    // Затем телепортируем через контроллер напрямую
                    _movementController.Warp(pos);
                }
                else
                {
                    Vector3 fallbackPos = new Vector3(93.11f, 0f, -100.17f);
                    reviveAbility.Revive(fallbackPos);
                    _movementController.Warp(fallbackPos);
                }
            }, _disposeCts.Token).Forget();
            
            globalPotionConsumeHandler.PotionConsumed.WithoutCurrent().ForEachAsync(PotionConsumed, _disposeCts.Token).Forget();
            globalCleanGardenBedHandler.GardenBedCleared.Listen(GardenBedCleared, _disposeCts.Token);
        }

        public async UniTask Initialize()
        {
            IsInitialized = true;
        }

        private void PotionConsumed(PotionType value)
        {
            var potionValue = _potionsConfig.GetPotion(value).Value;

            switch (value)
            {
                case PotionType.Health:
                    _healthHandler.Restore(potionValue);
                    Consume((ResourceId)"Resource_HealPotion");

                    break;
                case PotionType.Energy:
                    _energyHandler.Restore(potionValue);
                    Consume((ResourceId)"Resource_EnergyPotion");

                    break;
            }
        }

        private void Consume(ResourceId resource)
        {
            _resourcesService.DecreaseResourceValue(resource);
        }

        private void GardenBedCleared()
        {
            _energyHandler.Spend(_gardenConfig.GardenBedClearEnergy);
        }

        public void Dispose()
        {
            _disposeCts.Cancel();
            _disposeCts.Dispose();
        }
    }
}
