using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core.Interactions
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class InteractiveObject : MonoBehaviour
    {
        public float interactionDistance = 1;
        public float interactionDuration;
        public Sprite buttonSprite;
        public InteractionAnimation animation;

        [SerializeField] protected GameObject viewRoot;
        [SerializeField] private GameObject highLight;

        private SphereCollider _collider;

        public bool CanInteract { get; protected set; } = true;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Interactable");
        }

        public virtual void Select()
        {
            highLight.SetActive(true);
        }

        public virtual void Deselect()
        {
            highLight.SetActive(false);
        }

        public abstract UniTask Interaction();
    }
}