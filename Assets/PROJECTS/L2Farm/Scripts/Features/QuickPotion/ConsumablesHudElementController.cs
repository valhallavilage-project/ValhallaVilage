using System;
using System.Threading;
using CrossProject.Core;
using CrossProject.Core.InGameResources;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using VContainer.Unity;

namespace L2Farm
{
    public class ConsumablesHudElementController : IInitializable, IDisposable
    {
        private readonly UiService _uiService;
        private readonly GameStateManager _gameStateManager;
        private readonly IMainCharacterGlobalPotionConsumeHandler _mainCharacterPotionConsumeHandler;
        private readonly IResourcesService _resourcesService;

        private readonly CancellationTokenSource _disposeCts = new();
        private ConsumablesHudElement _view;
        private ResourceContentPart _resources;

        public bool IsInitialized { get; private set; }

        public ConsumablesHudElementController(UiService uiService, GameStateManager gameStateManager,
            IMainCharacterGlobalPotionConsumeHandler mainCharacterPotionConsumeHandler, IResourcesService resourcesService)
        {
            _uiService = uiService;
            _gameStateManager = gameStateManager;
            _mainCharacterPotionConsumeHandler = mainCharacterPotionConsumeHandler;
            _resourcesService = resourcesService;

            resourcesService.ResourceChanged.WithoutCurrent().ForEachAsync(ResourcesChanged, _disposeCts.Token).Forget();
        }

        public async UniTask Initialize()
        {
            var model = new ConsumablesHudElementModel();
            
            model.HealClicked.WithoutCurrent().ForEachAsync(HealClicked, _disposeCts.Token).Forget();
            model.EnergyClicked.WithoutCurrent().ForEachAsync(EnergyClicked, _disposeCts.Token).Forget();
            model.TimeClicked.WithoutCurrent().ForEachAsync(TimeClicked, _disposeCts.Token).Forget();
            
            _resources = _gameStateManager.State.Get<ResourceContentPart>();

            model.Resources = _resources;

            _view = await _uiService.TryOpen(model) as ConsumablesHudElement;

            IsInitialized = true;
        }

        private void HealClicked(bool _)
        {
            Consume((ResourceId)"Resource_HealPotion", PotionType.Health, _view.HealPotionConsumed);
        }

        private void EnergyClicked(bool _)
        {
            Consume((ResourceId)"Resource_EnergyPotion", PotionType.Energy, _view.ConsumeEnergyPotion);
        }

        private void TimeClicked(bool _)
        {
            Consume((ResourceId)"Resource_TimePotion", PotionType.Time, _view.ConsumeTimePotion);
        }

        private void Consume(ResourceId resourceId, PotionType type, Action onViewRefresh)
        {
            if (!_resources.Resources.TryGetValue(resourceId, out var count) || count <= 0)
                return;

            _mainCharacterPotionConsumeHandler.ConsumePotion(type);
            _resourcesService.ChangeResource(resourceId, -1);
            onViewRefresh?.Invoke();
        }

        private void ResourcesChanged((ResourceId id, int amount) resourceValue)
        {
            _view.UpdateResourceTexts();
        }

        public void Dispose()
        {
            _disposeCts.Cancel();
            _disposeCts.Dispose();
        }
    }
}
