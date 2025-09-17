using System.Linq;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Core.InGameResources
{
    public interface IResourcesService
    {
        Sprite GetSprite(ResourceId id);
        void ChangeResourceValue(ResourceId resourceId, int value);
        IReadOnlyAsyncReactiveProperty<(ResourceId, int)> ResourceChanged { get; }
    }

    public class ResourcesService : IResourcesService, IInitializable
    {
        private readonly AddressablesManager _addressablesManager;
        private readonly GameStateManager _gameStateManager;
        private ResourceSetConfig _resourceSetConfig;

        private readonly AsyncReactiveProperty<(ResourceId, int)> _resourceChanged = new(default);

        public IReadOnlyAsyncReactiveProperty<(ResourceId, int)> ResourceChanged => _resourceChanged;

        public bool IsInitialized { get; private set; }

        public ResourcesService(AddressablesManager addressablesManager, GameStateManager gameStateManager)
        {
            _addressablesManager = addressablesManager;
            _gameStateManager = gameStateManager;
        }

        public async UniTask Initialize()
        {
            _resourceSetConfig = await _addressablesManager.LoadAssetAsync<ResourceSetConfig>();
            IsInitialized = true;
        }

        public void ChangeResourceValue(ResourceId resourceId, int value)
        {
            var part = _gameStateManager.State.Get<ResourceContentPart>();
            part.Edit(resourceId, value);
            _gameStateManager.Save();

            _resourceChanged.Value = (resourceId, value);
        }

        public Sprite GetSprite(ResourceId id)
        {
            return _resourceSetConfig.items.FirstOrDefault(x => id == new ResourceId(x.id))?.icon;
        }
    }
}
