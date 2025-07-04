using Cysharp.Threading.Tasks;

namespace CrossProject.Core.Interactions
{
    public class ResourceInteractiveObject : InteractiveObject
    {
        public override async UniTask Interact()
        {
            await UniTask.WaitForSeconds(interactionDuration);
            
        }
    }
}