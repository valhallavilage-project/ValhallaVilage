using CrossProject.Core.Energy;
using CrossProject.Core.InGameResources;
using CrossProject.Core.Interactions;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace L2Farm.Features.ResourceHolder
{
    public class ResourceHolderInteractiveObject : AbstractInteractiveObject
    {
        [SerializeField] private ResourceContent content;
        [SerializeField] private int energyRequired;

        private IEnergyProvider _energyProvider;
        private GameStateManager _gameStateManager;

        public override bool CanInteract() => _energyProvider.CurrentValue >= energyRequired;

        protected override async UniTask AfterInteraction()
        {
            var part = _gameStateManager.State.Get<ResourceContentPart>();
            if (part.Resources.ContainsKey(content.Resource))
                part.Resources[content.Resource] += content.Amount;
            else
                part.Resources[content.Resource] = content.Amount;
            _gameStateManager.Save();
        }
    }
}
