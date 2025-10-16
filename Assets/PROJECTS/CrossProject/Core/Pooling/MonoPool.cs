using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrossProject.Core.Pooling
{
    public abstract class MonoPool<T> : MonoBehaviour, IPool where T : MonoBehaviour, IPoolElement
    {
        [SerializeField] private T prefab;
        [SerializeField] private int prewarmCount;
        [SerializeField] private bool activatePrewarmedElements;
        [SerializeField] private bool activateOnGet;
        [SerializeField] private bool autoExtend = true;
        [SerializeField] private Transform _elementsParent;
        [SerializeField] [HideIf("$autoExtend")] private bool reuse;
        [SerializeField] [HideIf("$autoExtend")] private int maxCount = 1;

        private Transform _t;
        private readonly LinkedList<T> _activeElements = new ();

        public LinkedList<T> ActiveElements => _activeElements;

        private T Create(bool active)
        {
            var result = Instantiate(prefab, _t);
            result.SetPool(this);
            result.gameObject.SetActive(active);
            _activeElements.AddLast(result);
            return result;
        }

        private void Awake()
        {
            _t = transform;

            if (prefab == null)
                throw new Exception($"{GetType().Name} is missing prefab for pooling!");

            if (prewarmCount < 0)
                prewarmCount = 0;

            if (maxCount < prewarmCount)
                maxCount = prewarmCount;

            for (int i = 0; i < prewarmCount; i++)
                Create(activatePrewarmedElements);
        }

        public T Get()
        {
            var result = _activeElements.FirstOrDefault(x => x.IsAvailableToGet);
            if (result == null)
            {
                if (autoExtend)
                    result = Create(activateOnGet);
                else if (reuse)
                {
                    result = _activeElements.First.Value;
                    _activeElements.RemoveFirst();
                }
            }

            _activeElements.AddLast(result);
            result.OnGet();
            return result;
        }

        public void Return(IPoolElement element)
        {
            var poolElement = element as T;
            var node = _activeElements.Find(poolElement);
            if (node == null)
                throw new Exception("Element is not cached in linked list!");

            _activeElements.Remove(node);
            element.OnReturn();

            if (_elementsParent != null)
            {
                poolElement.transform.SetParent(_elementsParent);
            }
        }
    }
}