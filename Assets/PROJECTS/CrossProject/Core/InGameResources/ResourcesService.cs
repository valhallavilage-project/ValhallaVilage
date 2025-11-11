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
        void SetNewResourceValue(ResourceId resourceId, int value);
        IReadOnlyAsyncReactiveProperty<(ResourceId, int)> ResourceChanged { get; }
        void ChangeResource(ResourceId resourceId, int changeValue);
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

        public void SetNewResourceValue(ResourceId resourceId, int value)
        {
            var part = _gameStateManager.State.Get<ResourceContentPart>();

            var oldValue = part.Get(resourceId);

            ChangeResource(resourceId, value - oldValue);
        }

        public void IncreaseResourceValue(ResourceId resourceId)
        {
            ChangeResource(resourceId, 1);
        }

        public void DecreaseResourceValue(ResourceId resourceId)
        {
            ChangeResource(resourceId, -1);
        }

        public void ChangeResource(ResourceId resourceId, int changeValue)
        {
            var part = _gameStateManager.State.Get<ResourceContentPart>();
            part.Edit(resourceId, changeValue);

            if (part.Resources[resourceId] <= 0)
            {
                part.Resources.Remove(resourceId);
            }
            
            _gameStateManager.Save();

            _resourceChanged.Value = (resourceId, changeValue);
        }

        public Sprite GetSprite(ResourceId id)
        {
            return _resourceSetConfig.items.First(x => id == new ResourceId(x.id))?.icon;
        }
    }
}
