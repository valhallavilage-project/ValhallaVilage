using UnityEngine;

namespace CrossProject.Core.Interactions
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class InteractiveObject : MonoBehaviour
    {
        [SerializeField] private GameObject highLight;

        private SphereCollider _collider;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
            gameObject.layer = LayerMask.NameToLayer("Interactable");
        }

        public virtual void Select()
        {
            highLight.SetActive(true);
            Debug.Log($"Select - {gameObject.name}");
        }
        
        public virtual void Deselect()
        {
            highLight.SetActive(false);
            Debug.Log($"Deselect - {gameObject.name}");
        }
        
    }
}