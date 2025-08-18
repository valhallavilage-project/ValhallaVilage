using System.Collections.Generic;
using CrossProject.Core;
using CrossProject.Core.SimpleMovement;
using CrossProject.Ui.Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Ui.Implementations
{
    public class JoystickController : IInitializable, ITickable, IBlockable, IJoystickValueProvider
    {
        private readonly UiService _uiService;
        private readonly AddressablesManager _addressablesManager;

        private Joystick _view;
        private bool _isDragActive;
        private readonly List<object> _blockers = new();

        public bool IsBlocked => _blockers.Count > 0;

        public Vector2 NormalizedVector2 => _view == null ? Vector2.zero : _view.NormalizedValue;

        public Vector3 NormalizedVector3OnPlain
        {
            get
            {
                if (_view == null)
                    return Vector3.zero;

                var value = _view.NormalizedValue;
                return new Vector3(value.x, 0, value.y);
            }
        }

        public bool IsInitialized { get; private set; }

        public JoystickController(
            UiService uiService,
            AddressablesManager addressablesManager)
        {
            _uiService = uiService;
            _addressablesManager = addressablesManager;
        }

        public async UniTask Initialize()
        {
            OpenJoystick();

            var screenRule = _uiService.GetRule(typeof(ScreenModel));
            screenRule.OnOpen += AddBlock;
            screenRule.OnClose += RemoveBlock;

            var popupRule = _uiService.GetRule(typeof(PopupModel));
            popupRule.OnOpen += AddBlock;
            popupRule.OnClose += RemoveBlock;

            IsInitialized = true;
        }

        public void AddBlock(object blockRequester)
        {
            if (_blockers.Count == 0)
                _uiService.HideHudElement<JoystickModel>();

            _blockers.Add(blockRequester);
        }

        public void RemoveBlock(object blockRequester)
        {
            _blockers.Remove(blockRequester);

            if (_blockers.Count == 0)
                _uiService.RevealHudElement<JoystickModel>();
        }

        private async void OpenJoystick()
        {
            var config = await _addressablesManager.LoadAssetAsync<JoystickConfig>(nameof(JoystickConfig));
            _view = await _uiService.TryOpen(JoystickModel.From(config)) as Joystick;
        }

        public void Tick()
        {
            if (_view == null)
            {
                return;
            }

            if (IsBlocked)
            {
                if (_view.NormalizedValue != Vector2.zero)
                    _view.EndDrag();

                return;
            }

            if (Input.GetMouseButtonDown(0) && _view.IsTouchInZone)
            {
                _isDragActive = true;
                _view.StartDrag();
            }

            if (Input.GetMouseButton(0) && _isDragActive)
            {
                _view.ProcessDrag();
            }

            if (Input.GetMouseButtonUp(0))
            {
                _view.EndDrag();
                _isDragActive = false;
            }
        }
    }
}