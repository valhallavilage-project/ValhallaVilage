using System.Collections.Generic;
using UnityEngine;

namespace CrossProject.Core.Interactions
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
    public class Interactor : MonoBehaviour
    {
        private readonly List<InteractiveObject> _objects = new();

        private SphereCollider _collider;
        private InteractiveObject _closest;

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<InteractiveObject>(out var interactableObject))
                _objects.Add(interactableObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<InteractiveObject>(out var interactableObject)
                && _objects.Contains(interactableObject))
            {
                _objects.Remove(interactableObject);
                if (_objects.Count == 0)
                    _closest = null;
            }
        }

        private void FixedUpdate()
        {
            float closestDistance = -1;
            InteractiveObject closest = null;
            foreach (var interactableObject in _objects)
            {
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

            if (_closest != closest && closest != null)
            {
                _closest = closest;
                _closest.Select();
            }
        }
    }
}