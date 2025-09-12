using System;
using CrossProject.Core.Interactions;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace CrossProject.Core
{
    public class MainCharacterAttackInteractionHandler : IDisposable, IInitializable
    {
        private Interactor _interactor;
        private readonly IAttackAbility _attackAbility;
        
        public bool IsInitialized { get; private set; }

        public MainCharacterAttackInteractionHandler(Interactor interactor, IAttackAbility attackAbility)
        {
            _interactor = interactor;
            _attackAbility = attackAbility;

            interactor.OnInteractionStart += InteractionStarted;
            interactor.OnInteractionEnd += InteractionEnded;
        }

        public UniTask Initialize()
        {
            IsInitialized = true;
            
            return UniTask.CompletedTask;
        }

        private void InteractionStarted(InteractionAnimation interaction)
        {
            if (interaction != InteractionAnimation.Attack)
            {
                return;
            }

            _attackAbility.BeginAttack();
        }

        private void InteractionEnded(InteractionAnimation interaction)
        {
            if (interaction != InteractionAnimation.Attack)
            {
                return;
            }

            _attackAbility.EndAttack();
        }

        public void Dispose()
        {
            _interactor.OnInteractionStart -= InteractionStarted;
            _interactor.OnInteractionEnd -= InteractionEnded;
            _interactor = null;
        }
    }
}
