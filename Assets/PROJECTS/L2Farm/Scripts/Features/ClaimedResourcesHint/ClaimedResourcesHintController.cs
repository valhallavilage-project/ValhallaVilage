using System;
using System.Threading;
using CrossProject.Core.InGameResources;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using VContainer.Unity;

namespace L2Farm.Features.ClaimerResourcesHint
{
    public class ClaimedResourcesHintController : IInitializable, IDisposable
    {
        private readonly UiService _uiService;
        private readonly ResourcesService _resourcesService;
        private readonly CancellationTokenSource _disposeCts = new();

        private ClaimedResourcesHint _view;

        public bool IsInitialized { get; private set; }

        public ClaimedResourcesHintController(UiService uiService, ResourcesService resourcesService)
        {
            _uiService = uiService;
            _resourcesService = resourcesService;
            
            resourcesService.ResourceChanged.WithoutCurrent().ForEachAsync(ResourceChanged, _disposeCts.Token).Forget();
        }

        private void ResourceChanged((ResourceId id, int amount) data)
        {
            if (data.amount == 0)
            {
                return;
            }

            _view.Spawn(_resourcesService.GetSprite(data.id), data.amount);
        }

        public async UniTask Initialize()
        {
            _view = await _uiService.TryOpen(new ClaimedResourcesHintModel()) as ClaimedResourcesHint;
            IsInitialized = true;
        }

        public void Dispose()
        {
            _disposeCts.Cancel();
            _disposeCts.Dispose();
        }
    }
}
