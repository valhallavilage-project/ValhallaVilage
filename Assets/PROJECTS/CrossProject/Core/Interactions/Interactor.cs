using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace CrossProject.Core.Interactions
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
    public class Interactor : MonoBehaviour
    {
        private readonly List<InteractiveObject> _objects = new();

        private SphereCollider _collider;

        public ReactiveProperty<InteractiveObject> Closest { get; } = new ();

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<InteractiveObject>(out var interactableObject) && interactableObject.CanInteract)
                _objects.Add(interactableObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<InteractiveObject>(out var interactableObject)
                && _objects.Contains(interactableObject))
            {
                interactableObject.Deselect();
                _objects.Remove(interactableObject);
                if (_objects.Count == 0)
                    Closest.Value = null;
            }
        }

        private void FixedUpdate()
        {
            float closestDistance = -1;
            InteractiveObject closest = null;
            foreach (var interactableObject in _objects)
            {
                if (interactableObject == null)
                    continue;

                if (!interactableObject.CanInteract)
                    continue;

                float distance = (interactableObject.transform.position - transform.position).sqrMagnitude;

                if (closest == null)
                {
                    closest = interactableObject;
                    closestDistance = distance;
                    continue;
                }

                if (distance < closestDistance)
                {
                    closest = interactableObject;
                    closestDistance = distance;
                }
            }

            if (Closest.Value != closest && closest != null)
            {
                Closest.Value?.Deselect();

                Closest.Value = closest;
                Closest.Value.Select();
            }
        }

        public async UniTask Interact()
        {
            await Closest.Value.Interaction();
            Closest.Value.Deselect();
        }
    }
}