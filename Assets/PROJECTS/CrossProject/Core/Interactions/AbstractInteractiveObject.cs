using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core.Interactions
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class AbstractInteractiveObject : MonoBehaviour
    {
        public float interactionDistance = 1;
        public float interactionDuration;
        public Sprite buttonSprite;
        public InteractionType animation;

        [SerializeField] protected GameObject viewRoot;
        [SerializeField] protected GameObject highLight;

        protected bool isBusy;

        private SphereCollider _collider;

        protected SphereCollider Collider => _collider;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Interactable");
        }

        public virtual bool CanSelect() => true;

        public virtual bool CanInteract() => !isBusy;

        public virtual void Select()
        {
            if (highLight != null) highLight.SetActive(true);
        }

        public virtual void Deselect()
        {
            if (highLight != null) highLight.SetActive(false);
        }

        protected abstract UniTask AfterInteraction();

        public async UniTask Interaction(CancellationToken cancellationToken = default)
        {
            isBusy = true;
            try
            {
                await UniTask.WaitForSeconds(interactionDuration, cancellationToken: cancellationToken);

                // Check cancellation before giving rewards
                cancellationToken.ThrowIfCancellationRequested();

                await AfterInteraction();
            }
            finally
            {
                isBusy = false;
            }
        }
    }
}