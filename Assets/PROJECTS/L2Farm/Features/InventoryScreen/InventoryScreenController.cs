using CrossProject.Core.InGameResources;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace L2Farm.Features.InventoryScreen
{
    public class InventoryScreenController : IInitializable
    {
        private readonly UiService _uiService;
        private readonly GameStateManager _gameStateManager;
        private readonly ResourcesService _resourcesService;

        private InventoryScreen _inventoryScreen;

        public bool IsInitialized { get; private set; }

        public InventoryScreenController(
            UiService uiService,
            GameStateManager gameStateManager,
            ResourcesService resourcesService)
        {
            _uiService = uiService;
            _gameStateManager = gameStateManager;
            _resourcesService = resourcesService;
        }

        private async void OpenScreen()
        {
            Debug.Log("Open Inventory Screen");
            _inventoryScreen = await _uiService.TryOpen(new InventoryScreenModel
            {
                gameStatePart = _gameStateManager.State.Get<ResourceContentPart>(),
                resourcesService = _resourcesService,
                Close = () => _uiService.Close(_inventoryScreen)
            }) as InventoryScreen;
        }

        public async UniTask Initialize()
        {
            await _uiService.TryOpen(new InventoryButtonModel(OpenScreen));
            IsInitialized = true;
        }
    }
}
