using System;
using System.Collections.Generic;
using CrossProject.Core.Interactions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public interface IInteractionHandler : IBlockable
    {
        IReadOnlyAsyncReactiveProperty<InteractionAnimation> InteractionStarted { get; }
        IReadOnlyAsyncReactiveProperty<InteractionAnimation> InteractionFinished { get; }
        IReadOnlyAsyncReactiveProperty<AbstractInteractiveObject> Closest { get; }
        IReadOnlyAsyncReactiveProperty<bool> InteractionLaunched { get; }
        bool IsInteractionInProcess { get; set; }

        void StartInteraction(InteractionAnimation interaction);
        void FinishInteraction(InteractionAnimation interaction);
        void SetClosestObject(AbstractInteractiveObject closest);
        UniTask Interact();
    }

    public class InteractionHandler : IInteractionHandler
    {
        private readonly HashSet<Type> _blockers = new();
        private bool _isInteractionInProcess;

        private readonly AsyncReactiveProperty<InteractionAnimation> _interactionStarted = new(default);
        private readonly AsyncReactiveProperty<InteractionAnimation> _interactionFinished = new(default);
        private readonly AsyncReactiveProperty<AbstractInteractiveObject> _closest = new(default);
        private readonly AsyncReactiveProperty<bool> _interactionLaunched = new(default);

        public IReadOnlyAsyncReactiveProperty<InteractionAnimation> InteractionStarted => _interactionStarted;
        public IReadOnlyAsyncReactiveProperty<InteractionAnimation> InteractionFinished => _interactionFinished;
        public IReadOnlyAsyncReactiveProperty<AbstractInteractiveObject> Closest => _closest;
        public IReadOnlyAsyncReactiveProperty<bool> InteractionLaunched => _interactionLaunched;

        public bool IsBlocked => _blockers.Count > 0;
        public bool IsInteractionInProcess { get; set; }

        public void StartInteraction(InteractionAnimation interaction)
        {
            _interactionStarted.Value = interaction;
        }

        public void FinishInteraction(InteractionAnimation interaction)
        {
            _interactionFinished.Value = interaction;
        }

        public void SetClosestObject(AbstractInteractiveObject closest)
        {
            _closest.Value = closest;
        }

        public void AddBlock(Type blockRequester)
        {
            if (!_blockers.Contains(blockRequester))
            {
                _blockers.Add(blockRequester);
                Debug.Log($"[{nameof(Interactor)}] : is blocked by : {blockRequester.Name}");
            }
        }

        public void RemoveBlock(Type blockRequester)
        {
            if (_blockers.Contains(blockRequester))
            {
                _blockers.Remove(blockRequester);
                Debug.Log($"[{nameof(Interactor)}] : is no longer blocked by : {blockRequester.Name}");
            }
        }

        public async UniTask Interact()
        {
            IsInteractionInProcess = true;
            _interactionLaunched.Value = true;

            await UniTask.WaitWhile(() => IsInteractionInProcess);
        }
    }
}
