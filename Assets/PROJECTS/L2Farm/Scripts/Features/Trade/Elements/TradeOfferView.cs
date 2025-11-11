using System.Collections.Generic;
using CrossProject.Core;
using CrossProject.Core.InGameResources;
using CrossProject.Core.Pooling;
using CrossProject.Core.SaveLoad;
using L2Farm.Features;
using L2Farm.Features.SimpleMonolog;
using UnityEngine;
using VContainer;

namespace L2Farm
{
    public class TradeOfferView : MonoBehaviour, IPoolElement
    {
        [SerializeField] private GameObject _requirementsPanel;
        [SerializeField] private GameObject _rewardPanel;
        [SerializeField] private GameObject _sellButtonUnavailability;

        private readonly List<ItemRequirement> _requirements = new List<ItemRequirement>();
        private ItemRequirementPool _itemsPool;
        private TradeOffer _offer;
        private ResourcesService _resourceService;
        private GameStateManager _gameStateManager;
        private IPool _viewPool;
        private ITimeService _timeService;
        private ISoftCurrencyHolder _softCurrencyHolder;

        public bool IsAvailableToGet { get; private set; }
        
        [Inject]
        private void AddDependencies(GameStateManager gameStateManager, ResourcesService resourceService,
            ITimeService timeService, ISoftCurrencyHolder softCurrencyHolder)
        {
            _softCurrencyHolder = softCurrencyHolder;
            _timeService = timeService;
            _gameStateManager = gameStateManager;
            _resourceService = resourceService;
        }

        public void Setup(TradeOffer offer, ItemRequirementPool pool)
        {
            _offer = offer;
            _itemsPool = pool;

            var isResourcesEnough = true;

            foreach (var offerRequirement in offer.Requirements)
            {
                var mainCharacterResourceAmount = _gameStateManager.State.Get<ResourceContentPart>().Get(offerRequirement.Resource);
                var itemRequirement = pool.Get();

                itemRequirement.Setup(new ConditionResourceData
                {
                    Count = offerRequirement.Count,
                    Icon = _resourceService.GetSprite(offerRequirement.Resource),
                    MainCharacterAmount = mainCharacterResourceAmount,
                    ResourcesType = MonologResourcesType.Demand
                });
                
                itemRequirement.transform.SetParent(_requirementsPanel.transform);

                isResourcesEnough &= mainCharacterResourceAmount >= offerRequirement.Count;
            }

            _sellButtonUnavailability.SetActive(!isResourcesEnough);
            
            var itemRequirementReward = pool.Get();
            
            itemRequirementReward.Setup(new ConditionResourceData
            {
                Count = offer.Reward,
                Icon = _resourceService.GetSprite((ResourceId)"Resource_Coin"),
                ResourcesType = MonologResourcesType.Give
            });
                
            itemRequirementReward.transform.SetParent(_rewardPanel.transform);
        }

        public void SellResources()
        {
            if (_offer == null)
            {
                return;
            }
            
            foreach (var offerRequirement in _offer.Requirements)
            {
                _resourceService.ChangeResource(offerRequirement.Resource, -offerRequirement.Count);
            }
            
            _softCurrencyHolder.ChangeValue(_offer.Reward);

            var finishedOffers = _gameStateManager.State.Get<TradeOffersStatePart>();
            
            finishedOffers.FinishedOffers.Add(new FinishedOffer()
            {
                DateFinished = _timeService.Now,
                Id = _offer.Id
            });

            _gameStateManager.Save();

            _offer = null;
            
            _viewPool.Return(this);
        }

        public void SetPool(IPool pool)
        {
            _viewPool = pool;
        }

        public void OnGet()
        {
            IsAvailableToGet = false;
            
            gameObject.SetActive(true);
        }

        public void OnReturn()
        {
            foreach (var itemRequirement in _requirements)
            {
                _itemsPool.Return(itemRequirement);
            }
            
            _requirements.Clear();

            _offer = null;
            
            gameObject.SetActive(false);
            
            IsAvailableToGet = true;
        }
    }
}
