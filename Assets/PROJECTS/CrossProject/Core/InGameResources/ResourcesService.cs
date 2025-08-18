using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Core.InGameResources
{
    public class ResourcesService : IInitializable
    {
        private readonly AddressablesManager _addressablesManager;

        private ResourceSetConfig _resourceSetConfig;

        public bool IsInitialized { get; private set; }

        public ResourcesService(AddressablesManager addressablesManager)
        {
            _addressablesManager = addressablesManager;
        }

        public async UniTask Initialize()
        {
            _resourceSetConfig = await _addressablesManager.LoadAssetAsync<ResourceSetConfig>();
            IsInitialized = true;
        }

        public Sprite GetSprite(ResourceId id)
        {
            return _resourceSetConfig.items.FirstOrDefault(x => id == new ResourceId(x.id))?.icon;
        }
    }
}
