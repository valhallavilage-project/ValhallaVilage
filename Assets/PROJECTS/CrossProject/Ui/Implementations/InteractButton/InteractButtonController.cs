using System;
using System.Collections.Generic;
using System.Threading;
using CrossProject.Core;
using CrossProject.Core.SimpleMovement;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Ui.Implementations.InteractButton
{
    public class InteractButtonController : IInitializable, IDisposable
    {
        private readonly UiService _uiService;
        private readonly IInteractionHandler _interactionHandler;
        private readonly JoystickController _joystickController;
        private readonly SimpleMovementController _simpleMovementController;

        private InteractButton _view;
        private CancellationTokenSource _cts = new ();
        private CancellationTokenSource _disposeCts = new ();

        public bool IsInitialized { get; private set; }

        public InteractButtonController(
            UiService uiService,
            IInteractionHandler interactionHandler,
            JoystickController joystickController,
            SimpleMovementController simpleMovementController)
        {
            _uiService = uiService;
            _interactionHandler = interactionHandler;
            _joystickController = joystickController;
            _simpleMovementController = simpleMovementController;
        }

        public async UniTask Initialize()
        {
            _view = await _uiService.TryOpen(new InteractButtonModel(null, null)) as InteractButton;
            
            _interactionHandler.Closest.WithoutCurrent().ForEachAsync(_ => UpdateButtonModel(), _disposeCts.Token).Forget();
            
            IsInitialized = true;
        }

        private async UniTask GetInteraction(CancellationToken cancellationToken)
        {
            if (!_interactionHandler.Closest.Value.CanInteract() || _interactionHandler.IsBlocked)
                return;

            _interactionHandler.AddBlock(GetType());
            _joystickController.AddBlock(GetType());
            _simpleMovementController.AddBlock(GetType());
            await _simpleMovementController.MoveTo(_interactionHandler.Closest.Value.transform.position, cancellationToken, _interactionHandler.Closest.Value.interactionDistance);
            await _interactionHandler.QueueInteraction();
            _interactionHandler.RemoveBlock(GetType());
            _joystickController.RemoveBlock(GetType());
            _simpleMovementController.RemoveBlock(GetType());
        }

        private void UpdateButtonModel()
        {
            if (_interactionHandler.Closest.Value == null)
            {
                Debug.Log($"[Interactions] null model");
                _view.BindModel(new InteractButtonModel(null, null));
                return;
            }

            Debug.Log($"[Interactions] {_interactionHandler.Closest.Value.name} ");
            _cts = new CancellationTokenSource();
            var model = new InteractButtonModel(_interactionHandler.Closest.Value.buttonSprite, () => GetInteraction(_cts.Token).Forget());
            _view.BindModel(model);
        }

        public void Dispose()
        {
            _disposeCts.Cancel();
            _disposeCts.Dispose();
        }
    }
}