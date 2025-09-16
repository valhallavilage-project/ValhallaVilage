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
        public InteractionAnimation animation;

        [SerializeField] protected GameObject viewRoot;
        [SerializeField] private GameObject highLight;

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
            highLight.SetActive(true);
        }

        public virtual void Deselect()
        {
            highLight.SetActive(false);
        }

        protected abstract UniTask AfterInteraction();

        public async UniTask Interaction()
        {
            isBusy = true;
            await UniTask.WaitForSeconds(interactionDuration);
            await AfterInteraction();
            isBusy = false;
        }
    }
}