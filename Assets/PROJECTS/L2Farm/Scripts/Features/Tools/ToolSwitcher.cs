using System.Linq;
using CrossProject.Core;
using CrossProject.Core.Interactions;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using L2Farm.Scripts;
using UnityEngine;
using VContainer;

namespace L2Farm.Features.Tools
{
    public class ToolSwitcher : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private ToolSetConfig toolSetConfig;

        private void Start()
        {
            Injector.Instance.Inject(this);
        }

        [Inject]
        private void Construct(IInteractionHandler interactionHandler)
        {
            interactionHandler.InteractionStarted.WithoutCurrent().ForEachAsync(InteractionStarted, gameObject.GetCancellationTokenOnDestroy()).Forget();
            interactionHandler.InteractionFinished.WithoutCurrent().ForEachAsync(InteractionFinished, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void InteractionStarted(InteractionAnimation interactionAnimation)
        {
            SwitchTo(interactionAnimation.GetToolId());
        }

        private void InteractionFinished(InteractionAnimation interactionAnimation)
        {
            SwitchTo(InteractionAnimation.Attack.GetToolId());
        }

        private void SwitchTo(ToolId id)
        {
            var config = toolSetConfig.items.FirstOrDefault(x => x.id == id);
            if (config == null)
                return;

            skinnedMeshRenderer.transform.localScale = Vector3.one;
            skinnedMeshRenderer.sharedMesh = config.mesh;
            skinnedMeshRenderer.material = config.material;
        }
    }
}
