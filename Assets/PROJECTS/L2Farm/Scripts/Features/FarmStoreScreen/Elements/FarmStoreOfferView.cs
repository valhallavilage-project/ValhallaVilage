using CrossProject.Core;
using CrossProject.Core.InGameResources;
using CrossProject.Core.Pooling;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace L2Farm
{
    public class FarmStoreOfferView : MonoBehaviour, IPoolElement
    {
        [SerializeField] private Image _productIcon;
        [SerializeField] private TMP_Text _productAmount;
        [SerializeField] private TMP_Text _price;
        [SerializeField] private GameObject _unavailable;
        
        private IResourcesService _resourcesService;
        private ISoftCurrencyHolder _softCurrencyHolder;
        private FarmStoreOffer _offer;

        public bool IsAvailableToGet { get; private set; }

        [Inject]
        private void AddDependencies(IResourcesService resourcesService, ISoftCurrencyHolder softCurrencyHolder)
        {
            _softCurrencyHolder = softCurrencyHolder;
            _resourcesService = resourcesService;
        }
        
        public void Setup(FarmStoreOffer offer)
        {
            _offer = offer;
            _productIcon.sprite = _resourcesService.GetSprite(offer.Product);
            _productAmount.text = offer.Count.ToString();
            _price.text = offer.Price.ToString();
            
            _unavailable.SetActive(_softCurrencyHolder.Amount < offer.Price);
        }

        public void SetPool(IPool pool)
        {
        }

        public void OnGet()
        {
            IsAvailableToGet = false;
            
            gameObject.SetActive(true);
        }

        public void OnReturn()
        {
            _offer = null;
            
            gameObject.SetActive(false);
            
            IsAvailableToGet = true;
        }
    }
}
