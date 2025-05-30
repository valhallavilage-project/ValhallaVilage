using CrossProject.Core.Pooling;
using UnityEngine;

namespace RUNNER.Scripts.Collectables
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class AbstractCollectable : MonoPoolElement
    {
        protected SphereCollider trigger;

        public override void OnGet()
        {
            base.OnGet();
            trigger ??= GetComponent<SphereCollider>();
            trigger.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                OnCollect();
        }

        protected abstract void OnCollect();
    }
}