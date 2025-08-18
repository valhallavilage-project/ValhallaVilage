using System.Threading;
using CrossProject.Core.Interactions;
using CrossProject.Core.SimpleMovement;
using CrossProject.Ui.Core;
using CrossProject.Ui.Implementations;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace L2Farm.Scripts.AutoLootButton
{
    public class AutoLootButtonController : IInitializable
    {
        private readonly UiService _uiService;
        private readonly Interactor _interactor;
        private readonly JoystickController _joystickController;
        private readonly SimpleMovementController _simpleMovementController;

        private AutoLootButton _view;
        private CancellationTokenSource _cts = new ();

        public bool IsInitialized { get; private set; }

        public AutoLootButtonController(
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
            var model = new AutoLootButtonModel
            {
                startAutoLoot = AutoLoot
            };
            _view = await _uiService.TryOpen(model) as AutoLootButton;
            IsInitialized = true;
        }

        private void AutoLoot()
        {
            AutoLootRoutine(_cts.Token).Forget();
        }

        private async UniTask AutoLootRoutine(CancellationToken cancellationToken)
        {
            _joystickController.AddBlock(this);
            _simpleMovementController.AddBlock(this);
            _view.gameObject.SetActive(false);
            while (_interactor.Closest.Value != null)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;

                await _simpleMovementController.MoveTo(_interactor.Closest.Value.transform.position, cancellationToken, _interactor.Closest.Value.interactionDistance);
                await _interactor.Interact();

                await UniTask.DelayFrame(1, PlayerLoopTiming.PostLateUpdate, cancellationToken);
            }

            _joystickController.RemoveBlock(this);
            _simpleMovementController.RemoveBlock(this);
            _view.gameObject.SetActive(true);
        }
    }
}
