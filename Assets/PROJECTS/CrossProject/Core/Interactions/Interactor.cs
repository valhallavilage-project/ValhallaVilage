using System.Collections.Generic;
using CrossProject.Core.Skins;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer;

namespace CrossProject.Core.Interactions
{
    [RequireComponent(typeof(SphereCollider))]
    public class Interactor : MonoBehaviour
    {
        private IPlayerSkinProvider _playerSkinProvider;

        private readonly List<AbstractInteractiveObject> _objects = new();

        private SphereCollider _collider;
        private IEnergyHandler _energyHandler;
        private IInteractionHandler _interactionHandler;
        private IExperienceHandler _experienceHandler;

        [Inject]
        private void Construct(IPlayerSkinProvider playerSkinProvider, IEnergyHandler energyHandler,
            IInteractionHandler interactionHandler, IExperienceHandler experienceHandler, IDieAbility dieAbility)
        {
            _playerSkinProvider = playerSkinProvider;
            _energyHandler = energyHandler;
            _interactionHandler = interactionHandler;
            _experienceHandler = experienceHandler;

            dieAbility.DeathBegan.Listen(MainCharacterDeathBegan, gameObject.GetCancellationTokenOnDestroy());
            interactionHandler.InteractionQueued.WithoutCurrent().ForEachAwaitAsync(Interact, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void MainCharacterDeathBegan()
        {
            foreach (var interactiveObject in _objects)
            {
                interactiveObject.Deselect();
            }
            
            _objects.Clear();
            _interactionHandler.SetClosestObject(null);
        }

        private void Awake()
        {
            _collider = GetComponent<SphereCollider>();
            _collider.isTrigger = true;
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
                    _interactionHandler.SetClosestObject(null);
            }
        }

        private void FixedUpdate()
        {
            if (_interactionHandler.IsBlocked)
                return;

            float closestDistance = -1;
            AbstractInteractiveObject closest = null;

            foreach (var interactableObject in _objects)
            {
                if (interactableObject == null)
                    continue;

                if (!interactableObject.CanInteract())
                    continue;

                var distance = (interactableObject.transform.position - transform.position).sqrMagnitude;

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

            if (_interactionHandler.Closest.Value != closest && closest != null)
            {
                _interactionHandler.Closest.Value?.Deselect();
                Debug.Log($"[{nameof(Interactor)}] : switch from \"{_interactionHandler.Closest.Value?.name}\" to \"{closest.name}\"");
                _interactionHandler.SetClosestObject(closest);
                closest.Select();
            }
            else if (closest == null)
            {
                _interactionHandler.SetClosestObject(null);
                Debug.Log($"[{nameof(Interactor)}] : there is no interactive objects nearby");
            }
        }

        private async UniTask Interact(bool _)
        {
            var animationName = _interactionHandler.Closest.Value.animation;

            // Fix: Rotate player to face the interaction object before starting animation
            if (_interactionHandler.Closest.Value != null)
            {
                var targetPosition = _interactionHandler.Closest.Value.transform.position;
                var direction = (targetPosition - transform.position);
                direction.y = 0; // Keep rotation on horizontal plane only

                if (direction.sqrMagnitude > 0.01f) // Only rotate if not too close
                {
                    var targetRotation = Quaternion.LookRotation(direction);
                    // Rotate player transform (parent of Interactor)
                    transform.parent.rotation = Quaternion.Slerp(transform.parent.rotation, targetRotation, 1f);
                }
            }

            if (animationName != InteractionType.Talk)
            {
                _playerSkinProvider.CurrentSkin.Animator.SetBool(animationName.ToString(), true);
                _interactionHandler.StartInteraction(animationName);
            }

            IResourceData resourceData = null;

            if (animationName is InteractionType.Chop or InteractionType.Gather or InteractionType.Pickaxe)
            {
                resourceData = _interactionHandler.Closest.Value as IResourceData;
            }

            await _interactionHandler.Closest.Value.Interaction();

            if (animationName is InteractionType.Chop or InteractionType.Gather or InteractionType.Pickaxe
                && resourceData != null)
            {
                _energyHandler.Spend(resourceData.EnergyRequired);
            }

            if (_interactionHandler.Closest.Value is IExperienceData experienceGiver)
            {
                _experienceHandler.GainXp(experienceGiver.PerformedTaskExperience);
            }

            if (animationName != InteractionType.Attack)
            {
                _interactionHandler.Closest.Value.Deselect();
            }

            if (animationName != InteractionType.Talk)
            {
                _playerSkinProvider.CurrentSkin.Animator.SetBool(animationName.ToString(), false);
                _interactionHandler.FinishInteraction(animationName);
            }

            _interactionHandler.IsInteractionInProcess = false;
        }
    }
}
