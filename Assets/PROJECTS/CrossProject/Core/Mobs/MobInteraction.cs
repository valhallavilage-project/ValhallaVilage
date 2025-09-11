using CrossProject.Core.Interactions;
using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public class MobInteraction : AbstractInteractiveObject
    {
        protected override UniTask AfterInteraction()
        {
            return UniTask.CompletedTask;
        }
    }
}
