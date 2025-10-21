using System;
using CrossProject.Core.Interactions;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using VContainer;

namespace CrossProject.Core
{
    public class MobInteraction : AbstractInteractiveObject
    {
        private bool _isDead;
        
        public override bool CanInteract()
        {
            return base.CanInteract() && !_isDead;
        }

        [Inject]
        private void AddDependencies(IDieAbility dieAbility)
        {
            dieAbility.DeathBegan.Listen(MobDie, gameObject.GetCancellationTokenOnDestroy());
        }

        private void OnEnable()
        {
            _isDead = false;
            Collider.enabled = true;
        }

        private void MobDie()
        {
            _isDead = true;
            Collider.enabled = false;
            Deselect();
        }

        protected override UniTask AfterInteraction()
        {
            return UniTask.CompletedTask;
        }
    }
}
