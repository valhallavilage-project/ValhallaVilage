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
        }

        private async UniTask GetInteraction()
        {
            _joystickController.AddBlock(this);
            _simpleMovementController.AddBlock(this);
            await _simpleMovementController.MoveTo(_interactor.Closest.Value.transform.position, _interactor.Closest.Value.interactionDistance);
            await _interactor.Interact();
            _joystickController.RemoveBlock(this);
            _simpleMovementController.RemoveBlock(this);
        }

        private void UpdateButtonModel()
        {
            if (_interactor.Closest.Value == null)
            {
                _view.BindModel(new InteractButtonModel(null, null));
                return;
            }

            var model = new InteractButtonModel(_interactor.Closest.Value.buttonSprite, () => GetInteraction().Forget())
                {
                    anchorMin = new Vector2(0.7f, 0.3f),
                    anchorMax = new Vector2(0.7f, 0.3f),
                    sizeDelta = new Vector2(150, 150)
                };
            _view.BindModel(model);
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable?.Dispose();
        }
    }
}