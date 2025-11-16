using CrossProject.Ui.Core;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm
{
    public class FarmStoreScreen : ScreenView<FarmStoreScreenModel>, LoopScrollPrefabSource, LoopScrollDataSource
    {
        
        [SerializeField] private FarmStoreOffersConfig _offersConfig;
        [SerializeField] private LoopScrollRect _scroll;
        [SerializeField] private FarmStoreOfferViewPool _farmStoreOfferViewPool;
        [SerializeField] private Button _closeButton;

        protected override void OnBind()
        {
            base.OnBind();
            
            _closeButton.SetUniqueCallback(Model.Close);
        }

        private void Start()
        {
            _scroll.prefabSource = this;
            _scroll.dataSource = this;
            _scroll.totalCount = _offersConfig.Offers.Count;
            _scroll.RefillCells();
        }

        public GameObject GetObject(int index)
        {
            var offerView = _farmStoreOfferViewPool.Get();

            return offerView.gameObject;
        }

        public void ReturnObject(Transform poolObjectTransform)
        {
            var offerView = poolObjectTransform.GetComponent<FarmStoreOfferView>();

            _farmStoreOfferViewPool.Return(offerView);
        }

        public void ProvideData(Transform poolObjectTransform, int index)
        {
            var offerView = poolObjectTransform.GetComponent<FarmStoreOfferView>();
            
            offerView.Setup(_offersConfig.Offers[index]);
        }
    }
}
