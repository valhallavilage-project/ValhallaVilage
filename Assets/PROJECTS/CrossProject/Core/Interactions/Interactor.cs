using System.Collections.Generic;
using R3;
using UnityEngine;

namespace CrossProject.Core.Interactions
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
    public class Interactor : MonoBehaviour
    {
        private readonly List<InteractiveObject> _objects = new();

        private SphereCollider _collider;
        private Transform _selectIndicator;

        [SerializeField]
        private GameObject selectIndicatorPrefab;

        public ReactiveProperty<InteractiveObject> Closest { get; } = new ();

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
            if (_selectIndicator != null)
            {
                _selectIndicator = Instantiate(selectIndicatorPrefab, Vector3.zero, Quaternion.identity, transform).transform;
                _selectIndicator.gameObject.SetActive(false);
            }
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
                    Closest.Value = null;
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

            if (Closest.Value != closest && closest != null)
            {
                Closest.Value.Deselect();

                Closest.Value = closest;
                Closest.Value.Select();

                if (_selectIndicator != null)
                {
                    if (!_selectIndicator.gameObject.activeSelf)
                        _selectIndicator.gameObject.SetActive(true);
                    _selectIndicator.position = Closest.Value.transform.position;
                    _selectIndicator.localScale = Vector3.one * Closest.Value.selectorScale;
                }
            }
        }
    }
}