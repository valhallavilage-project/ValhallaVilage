using System;
using System.Collections.Generic;
using CrossProject.Core.Interactions;
using CrossProject.Core.SimpleMovement;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using R3;
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

        private async UniTask GetInteraction(InteractiveObject interactiveObject)
        {
            _joystickController.RequestBlock(this);
            _simpleMovementController.RequestBlock(this);
            await _simpleMovementController.MoveTo(interactiveObject.transform.position);
            await interactiveObject.Interact();
            _joystickController.ReleaseBlock(this);
            _simpleMovementController.RequestBlock(this);
        }

        private void UpdateButtonModel(InteractiveObject interactiveObject)
        {
            _view.BindModel(new InteractButtonModel(interactiveObject.buttonSprite, () => GetInteraction(interactiveObject).Forget()));
        }

        public async void Initialize()
        {
            _view = await _uiService.TryOpen(new InteractButtonModel(null, null)) as InteractButton;
            _disposables.Add(_interactor.Closest.Subscribe(UpdateButtonModel));
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable?.Dispose();
        }
    }
}