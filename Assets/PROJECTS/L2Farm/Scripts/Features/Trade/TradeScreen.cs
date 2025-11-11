using System;
using System.Collections.Generic;
using System.Linq;
using CrossProject.Core;
using CrossProject.Core.SaveLoad;
using CrossProject.Ui.Core;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using Random = UnityEngine.Random;

namespace L2Farm
{
    public class TradeScreen : ScreenView<TradeScreenModel>
    {
        [SerializeField] private TradeOffersConfig _offersConfig;
        [SerializeField] private GameObject _offersPanel;
        [SerializeField] private TradeOfferViewPool _tradeOfferViewPool;
        [SerializeField] private ItemRequirementPool _itemRequirementPool;
        [SerializeField] private Button _closeBtn;
        [SerializeField] private GameObject _noTradeOffersState;
        
        private GameStateManager _gameStateManager;
        private ITimeService _timeService;
        
        private readonly List<TradeOfferView> _createdOfferViews = new();

        [Inject]
        private void AddDependencies(GameStateManager gameStateManager, ITimeService timeService)
        {
            _timeService = timeService;
            _gameStateManager = gameStateManager;
        }
        
        protected override void OnBind()
        {
            base.OnBind();
            
            _closeBtn.SetUniqueCallback(Model.Close);

            var offersStatePart = _gameStateManager.State.Get<TradeOffersStatePart>();

            var lastOffersUpdateDate = offersStatePart.LastOffersUpdateDate;

            var offers = new List<TradeOffer>();

            if (_timeService.Now.DayOfYear > lastOffersUpdateDate.DayOfYear)
            {
                offersStatePart.DailyOffers.Clear();
                
                var maxOffers = Math.Min(_offersConfig.Offers.Count, _offersConfig.OffersPerDay);
                
                while (offers.Count < maxOffers)
                {
                    var offer = _offersConfig.Offers[(int)(Random.value * _offersConfig.Offers.Count)];

                    if (offers.Count == 0 || offers.All(o => o.Id != offer.Id))
                    {
                        offers.Add(offer);
                        offersStatePart.DailyOffers.Add(offer.Id);
                    }
                }
                
                _gameStateManager.Save();
            }
            else
            {
                foreach (var offerId in offersStatePart.DailyOffers)
                {
                    var configOffer = _offersConfig.Offers.First(o => o.Id == offerId);
                    
                    offers.Add(configOffer);
                }
            }
            
            foreach (var tradeOffer in offers)
            {
                if (offersStatePart.FinishedOffers.Any(fo => fo.Id == tradeOffer.Id && fo.DateFinished.DayOfYear >= _timeService.Now.DayOfYear))
                {
                    continue;
                }
                
                var tradeOfferView = _tradeOfferViewPool.Get();
                
                tradeOfferView.transform.SetParent(_offersPanel.transform);
                tradeOfferView.Setup(tradeOffer, _itemRequirementPool);
                
                _createdOfferViews.Add(tradeOfferView);
            }
            
            _noTradeOffersState.SetActive(_createdOfferViews.Count == 0);
        }

        public void Clear()
        {
            foreach (var tradeOfferView in _createdOfferViews)
            {
                if (tradeOfferView.transform.parent == _offersPanel.transform)
                {
                    _tradeOfferViewPool.Return(tradeOfferView);
                }
            }
        }
    }
}
