using CrossProject.Core.InGameResources;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace L2Farm.Features.ClaimerResourcesHint
{
    public class ClaimedResourcesHintController : IInitializable
    {
        private readonly UiService _uiService;
        private readonly ResourcesService _resourcesService;
        private readonly GameStateManager _gameStateManager;

        private ClaimedResourcesHint _view;

        public bool IsInitialized { get; private set; }

        public ClaimedResourcesHintController(
            UiService uiService,
            ResourcesService resourcesService,
            GameStateManager gameStateManager)
        {
            _uiService = uiService;
            _resourcesService = resourcesService;
            _gameStateManager = gameStateManager;
        }

        private void OnResourceChange(ResourceId id, int amount)
        {
            if (amount < 1)
                return;

            _view.Spawn(_resourcesService.GetSprite(id), amount);
        }

        public async UniTask Initialize()
        {
            _view = await _uiService.TryOpen(new ClaimedResourcesHintModel()) as ClaimedResourcesHint;
            var part = _gameStateManager.State.Get<ResourceContentPart>();
            part.OnResourceChange += OnResourceChange;
            IsInitialized = true;
        }
    }
}
