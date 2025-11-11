using System;
using System.Collections.Generic;
using CrossProject.Core.InGameResources;
using UnityEngine;

namespace L2Farm
{
    [Serializable]
    public class TradeOfferResourceRequirement
    {
        [SerializeField] private ResourceId _resource;
        [SerializeField] private int _count;

        public ResourceId Resource => _resource;
        public int Count => _count;
    }

    [Serializable]
    public class TradeOffer
    {
        [SerializeField] private string _id;
        [SerializeField] private List<TradeOfferResourceRequirement> _requirements;
        [SerializeField] private int _reward;

        public string Id => _id;
        public List<TradeOfferResourceRequirement> Requirements => _requirements;
        public int Reward => _reward;
    }

    [CreateAssetMenu(fileName = nameof(TradeOffersConfig), menuName = "ScriptableObjects/Configs/TradeOffers")]
    public class TradeOffersConfig : ScriptableObject
    {
        [SerializeField] private int _offersPerDay = 3;
        [SerializeField] private List<TradeOffer> _offers;

        public List<TradeOffer> Offers => _offers;
        public int OffersPerDay => _offersPerDay;
    }
}
