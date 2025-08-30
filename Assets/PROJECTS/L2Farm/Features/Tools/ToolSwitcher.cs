using System.Linq;
using CrossProject.Core;
using CrossProject.Core.Interactions;
using CrossProject.Extensions;
using UnityEngine;
using VContainer;

namespace L2Farm.Features.Tools
{
    public class ToolSwitcher : MonoBehaviour
    {
        public static ToolSwitcher Instance;

        private Interactor _interactor;

        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private ToolSetConfig toolSetConfig;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            Injector.Instance?.Inject(this);
        }

        [Inject]
        private void Construct(Interactor interactor)
        {
            _interactor = interactor;
            _interactor.OnInteractionStart += OnInteractionStart;
            _interactor.OnInteractionEnd += OnInteractionEnd;
        }

        private void OnDestroy()
        {
            _interactor.OnInteractionStart -= OnInteractionStart;
            _interactor.OnInteractionEnd -= OnInteractionEnd;
        }

        private void OnInteractionStart(InteractionAnimation interactionAnimation)
        {
            SwitchTo(interactionAnimation.GetToolId());
        }

        private void OnInteractionEnd(InteractionAnimation interactionAnimation)
        {
            SwitchTo(InteractionAnimation.Attack.GetToolId());
        }

        public void SwitchTo(ToolId id)
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
