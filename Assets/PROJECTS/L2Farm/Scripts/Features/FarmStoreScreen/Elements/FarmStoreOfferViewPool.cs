using CrossProject.Core.Pooling;
using UnityEngine;
using UnityEngine.UI;

namespace L2Farm
{
    public class FarmStoreOfferViewPool : MonoPool<FarmStoreOfferView>
    {
        public GameObject GetObject(int index)
        {
            return Get().gameObject;
        }

        public void ReturnObject(Transform trans)
        {
            var poolElement = trans.GetComponentInChildren<IPoolElement>();
            
            Return(poolElement);
        }

        public void ProvideData(Transform transform, int idx)
        {
            throw new System.NotImplementedException();
        }
    }
}
