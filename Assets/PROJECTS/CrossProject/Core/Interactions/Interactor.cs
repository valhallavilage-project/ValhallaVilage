using System;
using System.Collections.Generic;
using CrossProject.Core.Skins;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer;

namespace CrossProject.Core.Interactions
{
    [RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
    public class Interactor : MonoBehaviour, IBlockable
    {
        private IPlayerSkinProvider _playerSkinProvider;

        private readonly List<AbstractInteractiveObject> _objects = new();

        private SphereCollider _collider;
        private HashSet<Type> _blockers = new ();

        public ReactiveProperty<AbstractInteractiveObject> Closest { get; } = new ();
        public bool IsBlocked => _blockers.Count > 0;

        public event Action<InteractionAnimation> OnInteractionStart;
        public event Action<InteractionAnimation> OnInteractionEnd;

        public void AddBlock(object blockRequester)
        {
            var type = blockRequester.GetType();
            if (!_blockers.Contains(type))
            {
                _blockers.Add(type);
                Debug.Log($"[{nameof(Interactor)}] : is blocked by : {type.Name}");
            }
        }

        public void RemoveBlock(object blockRequester)
        {
            var type = blockRequester.GetType();
            if (_blockers.Contains(type))
            {
                _blockers.Remove(type);
                Debug.Log($"[{nameof(Interactor)}] : is no longer blocked by : {type.Name}");
            }
        }

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
        }

        private void Start()
        {
            Injector.Instance.Inject(this);
        }

        [Inject]
        private void Construct(IPlayerSkinProvider playerSkinProvider)
        {
            _playerSkinProvider = playerSkinProvider;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<AbstractInteractiveObject>(out var interactableObject) && interactableObject.CanInteract())
                _objects.Add(interactableObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<AbstractInteractiveObject>(out var interactableObject)
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
            if (IsBlocked)
                return;

            float closestDistance = -1;
            AbstractInteractiveObject closest = null;
            foreach (var interactableObject in _objects)
            {
                if (interactableObject == null)
                    continue;

                if (!interactableObject.CanInteract())
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
                Debug.Log($"[{nameof(Interactor)}] : switch from \"{Closest.Value?.name}\" to \"{closest.name}\"");
                Closest.Value = closest;
                Closest.Value.Select();
            }
            else if (closest == null)
            {
                if (Closest.Value != null)
                    Debug.Log($"[{nameof(Interactor)}] : there is no interactive objects nearby");
                Closest.Value = null;
            }
        }

        public async UniTask Interact()
        {
            var animationName = Closest.Value.animation;
            if (animationName != InteractionAnimation.Talk)
            {
                _playerSkinProvider.CurrentSkin.Animator.SetBool(animationName.ToString(), true);
                OnInteractionStart?.Invoke(animationName);
            }

            await Closest.Value.Interaction();
            Closest.Value.Deselect();

            if (animationName != InteractionAnimation.Talk)
            {
                _playerSkinProvider.CurrentSkin.Animator.SetBool(animationName.ToString(), false);
                OnInteractionEnd?.Invoke(animationName);
            }
        }
    }
}