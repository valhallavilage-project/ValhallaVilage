using System;
using System.Collections.Generic;
using System.Linq;
using CrossProject.Ui.Core;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace L2Farm
{
    public class TradeScreen : ScreenView<TradeScreenModel>, LoopScrollPrefabSource, LoopScrollDataSource
    {
        [SerializeField] private TradeOffersConfig _offersConfig;
        [SerializeField] private LoopScrollRect _scroll;
        [SerializeField] private TradeOfferViewPool _tradeOfferViewPool;
        [SerializeField] private ItemRequirementPool _itemRequirementPool;
        [SerializeField] private Button _closeBtn;
        [SerializeField] private GameObject _noTradeOffersState;

        private readonly List<TradeOffer> _activeOffers = new();

        protected override void OnBind()
        {
            base.OnBind();

            _closeBtn.SetUniqueCallback(Model.Close);
        }

        private void Start()
        {
            FilterOffers();

            _scroll.prefabSource = this;
            _scroll.dataSource = this;
            _scroll.totalCount = _activeOffers.Count;
            _scroll.RefillCells();

            _noTradeOffersState.SetActive(_activeOffers.Count == 0);
        }

        private void FilterOffers()
        {
            var offersStatePart = Model.GameStateManager.State.Get<TradeOffersStatePart>();

            var lastOffersUpdateDate = offersStatePart.LastOffersUpdateDate;

            var offers = new List<TradeOffer>();

            if (Model.TimeService.Now.DayOfYear > lastOffersUpdateDate.DayOfYear)
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

                Model.GameStateManager.Save();
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
                if (offersStatePart.FinishedOffers.Any(fo => fo.Id == tradeOffer.Id &&
                                                             fo.DateFinished.DayOfYear >= Model.TimeService.Now.DayOfYear))
                {
                    continue;
                }

                _activeOffers.Add(tradeOffer);
            }
        }

        public GameObject GetObject(int index)
        {
            var offerView = _tradeOfferViewPool.Get();

            return offerView.gameObject;
        }

        public void ReturnObject(Transform poolObjectTransform)
        {
            var offerView = poolObjectTransform.GetComponent<TradeOfferView>();

            _tradeOfferViewPool.Return(offerView);
        }

        public void ProvideData(Transform poolObjectTransform, int index)
        {
            var offerView = poolObjectTransform.GetComponent<TradeOfferView>();

            offerView.Setup(_activeOffers[index], _itemRequirementPool);
        }
    }
}
