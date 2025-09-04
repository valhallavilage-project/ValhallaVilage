using System;
using System.Collections.Generic;
using System.Threading;
using CrossProject.Core.Interactions;
using CrossProject.Core.SimpleMovement;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Ui.Implementations.InteractButton
{
    public class InteractButtonController : IInitializable, IDisposable
    {
        private readonly UiService _uiService;
        private readonly Interactor _interactor;
        private readonly JoystickController _joystickController;
        private readonly SimpleMovementController _simpleMovementController;
        private readonly List<IDisposable> _disposables = new();

        private InteractButton _view;
        private CancellationTokenSource _cts = new ();

        public bool IsInitialized { get; private set; }

        public InteractButtonController(
            UiService uiService,
            Interactor interactor,
            JoystickController joystickController,
            SimpleMovementController simpleMovementController)
        {
            _uiService = uiService;
            _interactor = interactor;
            _joystickController = joystickController;
            _simpleMovementController = simpleMovementController;
        }

        public async UniTask Initialize()
        {
            _view = await _uiService.TryOpen(new InteractButtonModel(null, null)) as InteractButton;
            _disposables.Add(_interactor.Closest.Subscribe(_ => UpdateButtonModel()));
            IsInitialized = true;
        }

        private async UniTask GetInteraction(CancellationToken cancellationToken)
        {
            if (!_interactor.Closest.Value.CanInteract() || _interactor.IsBlocked)
                return;

            _interactor.AddBlock(this);
            _joystickController.AddBlock(this);
            _simpleMovementController.AddBlock(this);
            await _simpleMovementController.MoveTo(_interactor.Closest.Value.transform.position, cancellationToken, _interactor.Closest.Value.interactionDistance);
            await _interactor.Interact();
            _interactor.RemoveBlock(this);
            _joystickController.RemoveBlock(this);
            _simpleMovementController.RemoveBlock(this);
        }

        private void UpdateButtonModel()
        {
            if (_interactor.Closest.Value == null)
            {
                Debug.Log($"[Interactions] null model");
                _view.BindModel(new InteractButtonModel(null, null));
                return;
            }

            Debug.Log($"[Interactions] {_interactor.Closest.Value.name} ");
            _cts = new CancellationTokenSource();
            var model = new InteractButtonModel(_interactor.Closest.Value.buttonSprite, () => GetInteraction(_cts.Token).Forget());
            _view.BindModel(model);
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable?.Dispose();
        }
    }
}