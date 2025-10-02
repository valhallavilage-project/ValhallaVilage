using CrossProject.Core.Pooling;
using UnityEngine;

namespace L2Farm.Features
{
    public class ActiveQuestsItem : MonoBehaviour, IPoolElement
    {
        private IPool _pool;
        
        public bool IsAvailableToGet { get; private set; }

        public void SetPool(IPool pool)
        {
            _pool = pool;
        }

        public void OnGet()
        {
            IsAvailableToGet = false;
        }

        public void OnReturn()
        {
            IsAvailableToGet = true;
        }

        public void Clear()
        {
            _pool.Return(this);
        }
    }
}
