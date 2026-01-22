using System;
using System.Collections.Generic;
using System.Threading;
using CrossProject.Core.Interactions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public interface IInteractionHandler : IBlockable
    {
        IReadOnlyAsyncReactiveProperty<InteractionType> InteractionStarted { get; }
        IReadOnlyAsyncReactiveProperty<InteractionType> InteractionFinished { get; }
        IReadOnlyAsyncReactiveProperty<AbstractInteractiveObject> Closest { get; }
        IReadOnlyAsyncReactiveProperty<bool> InteractionQueued { get; }
        bool IsInteractionInProcess { get; set; }
        bool IsMovingToTarget { get; set; }
        bool IsCancelled { get; }
        CancellationToken CancellationToken { get; }

        void StartInteraction(InteractionType interaction);
        void FinishInteraction(InteractionType interaction);
        void SetClosestObject(AbstractInteractiveObject closest);
        UniTask QueueInteraction();
        void CancelInteraction();
        void ResetCancellation();
    }

    public class InteractionHandler : IInteractionHandler
    {
        private readonly HashSet<Type> _blockers = new();
        private CancellationTokenSource _cts = new();

        private readonly AsyncReactiveProperty<InteractionType> _interactionStarted = new(default);
        private readonly AsyncReactiveProperty<InteractionType> _interactionFinished = new(default);
        private readonly AsyncReactiveProperty<AbstractInteractiveObject> _closest = new(default);
        private readonly AsyncReactiveProperty<bool> _interactionQueued = new(default);

        public IReadOnlyAsyncReactiveProperty<InteractionType> InteractionStarted => _interactionStarted;
        public IReadOnlyAsyncReactiveProperty<InteractionType> InteractionFinished => _interactionFinished;
        public IReadOnlyAsyncReactiveProperty<AbstractInteractiveObject> Closest => _closest;
        public IReadOnlyAsyncReactiveProperty<bool> InteractionQueued => _interactionQueued;

        public bool IsBlocked => _blockers.Count > 0;
        public bool IsInteractionInProcess { get; set; }
        public bool IsMovingToTarget { get; set; }
        public bool IsCancelled { get; private set; }
        public CancellationToken CancellationToken => _cts.Token;

        public void StartInteraction(InteractionType interaction)
        {
            _interactionStarted.Value = interaction;
        }

        public void FinishInteraction(InteractionType interaction)
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

        public async UniTask QueueInteraction()
        {
            IsInteractionInProcess = true;
            _interactionQueued.Value = true;

            await UniTask.WaitWhile(() => IsInteractionInProcess);
        }

        public void CancelInteraction()
        {
            if (!IsInteractionInProcess && !IsMovingToTarget)
                return;

            IsCancelled = true;
            try
            {
                _cts?.Cancel();
            }
            catch { }
            IsInteractionInProcess = false;
            IsMovingToTarget = false;
            Debug.Log("[InteractionHandler] Interaction cancelled");
        }

        public void ResetCancellation()
        {
            IsCancelled = false;
            try
            {
                _cts?.Dispose();
            }
            catch { }
            _cts = new CancellationTokenSource();
        }
    }
}
