using System;
using System.Threading;
using CrossProject.Core.InGameResources;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer.Unity;

namespace L2Farm.Features.InventoryScreen
{
    public class InventoryScreenController : IInitializable, IDisposable
    {
        private readonly UiService _uiService;
        private readonly GameStateManager _gameStateManager;
        private readonly ResourcesService _resourcesService;
        private readonly CancellationTokenSource _disposeCts = new ();

        private InventoryScreen _inventoryScreen;
        private InventoryButtonModel _buttonModel;

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

        private async UniTask OpenScreen()
        {
            _inventoryScreen = await _uiService.TryOpen(new InventoryScreenModel
            {
                gameStatePart = _gameStateManager.State.Get<ResourceContentPart>(),
                resourcesService = _resourcesService,
                Close = () => _uiService.Close(_inventoryScreen)
            }) as InventoryScreen;
        }

        public async UniTask Initialize()
        {
            _buttonModel = new InventoryButtonModel();

            _buttonModel.Clicked.Listen(OpenScreen, _disposeCts.Token);
            
            await _uiService.TryOpen(_buttonModel);
            
            IsInitialized = true;
        }

        public void Dispose()
        {
            _disposeCts.Cancel();
            _disposeCts.Dispose();
        }
    }
}
