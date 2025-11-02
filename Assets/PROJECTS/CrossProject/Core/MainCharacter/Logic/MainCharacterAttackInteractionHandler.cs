using System;
using System.Threading;
using CrossProject.Core.Interactions;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using VContainer.Unity;

namespace CrossProject.Core
{
    public class MainCharacterAttackInteractionHandler : IDisposable, IInitializable
    {
        private readonly IAttackAbility _attackAbility;
        private readonly CancellationTokenSource _disposeCts;
        
        public bool IsInitialized { get; private set; }

        public MainCharacterAttackInteractionHandler(IInteractionHandler interactionHandler, IAttackAbility attackAbility)
        {
            _attackAbility = attackAbility;

            _disposeCts = new CancellationTokenSource();

            interactionHandler.InteractionStarted.WithoutCurrent().ForEachAsync(InteractionStarted, _disposeCts.Token).Forget();
            interactionHandler.InteractionFinished.WithoutCurrent().ForEachAsync(InteractionFinished, _disposeCts.Token).Forget();
        }

        public UniTask Initialize()
        {
            IsInitialized = true;
            
            return UniTask.CompletedTask;
        }

        private void InteractionStarted(InteractionType interaction)
        {
            if (interaction != InteractionType.Attack)
            {
                return;
            }

            _attackAbility.BeginAttack();
        }

        private void InteractionFinished(InteractionType interaction)
        {
            if (interaction != InteractionType.Attack)
            {
                return;
            }

            _attackAbility.EndAttack();
        }

        public void Dispose()
        {
            _disposeCts.Cancel();
            _disposeCts.Dispose();
        }
    }
}
