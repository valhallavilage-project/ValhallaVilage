using System;
using CrossProject.Core.Interactions;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using VContainer;

namespace CrossProject.Core
{
    public class MobInteraction : AbstractInteractiveObject
    {
        [Inject]
        private void AddDependencies(IDieAbility dieAbility)
        {
            dieAbility.DeathBegan.WithoutCurrent().ForEachAsync(MobDie, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void OnEnable()
        {
            Collider.enabled = true;
        }

        private void MobDie(bool _)
        {
            Collider.enabled = false;
            Deselect();
        }

        protected override UniTask AfterInteraction()
        {
            return UniTask.CompletedTask;
        }
    }
}
