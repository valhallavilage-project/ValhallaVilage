using System;
using System.Collections.Generic;
using CrossProject.Core.InGameResources;
using UnityEngine;

namespace L2Farm
{
    [Serializable]
    public class FarmStoreOffer
    {
        [SerializeField] private ResourceId _product;
        [SerializeField] private int _count;
        [SerializeField] private int _price;

        public ResourceId Product => _product;
        public int Count => _count;
        public int Price => _price;
    }

    [CreateAssetMenu(fileName = nameof(FarmStoreOffersConfig), menuName = "ScriptableObjects/Configs/FarmStoreOffers")]
    public class FarmStoreOffersConfig : ScriptableObject
    {
        [SerializeField] private List<FarmStoreOffer> _offers;

        public List<FarmStoreOffer> Offers => _offers;
    }
}
