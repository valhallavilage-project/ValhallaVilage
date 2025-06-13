using System.Collections.Generic;
using CrossProject.Core;
using CrossProject.Ui.Core;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Ui.Implementations
{
    public class JoystickController : IInitializable, ITickable, IBlocksManager, IJoystickValueProvider
    {
        private readonly UiService _uiService;
        private readonly AddressablesManager _addressablesManager;

        private Joystick _view;
        private readonly List<object> _blockers = new();

        public bool IsBlocked => _blockers.Count > 0;
        public Vector2 NormalizedValue => _view.NormalizedValue;

        public JoystickController(
            UiService uiService,
            AddressablesManager addressablesManager)
        {
            _uiService = uiService;
            _addressablesManager = addressablesManager;
        }

        public void RequestBlock(object blockRequester)
        {
            if (_blockers.Count == 0)
                _uiService.HideHudElement<JoystickModel>();

            _blockers.Add(blockRequester);
        }

        public void ReleaseBlock(object blockRequester)
        {
            _blockers.Remove(blockRequester);

            if (_blockers.Count == 0)
                _uiService.RevealHudElement<JoystickModel>();
        }

        private async void OpenJoystick()
        {
            var config = await _addressablesManager.LoadAssetAsync<JoystickConfig>(nameof(JoystickConfig));
            _view ??= await _uiService.TryOpen(JoystickModel.From(config)) as Joystick;
        }

        public void Initialize()
        {
            OpenJoystick();
        }

        public void Tick()
        {
            if (IsBlocked)
            {
                if (_view.NormalizedValue != Vector2.zero)
                    _view.EndDrag();

                return;
            }

            if (Input.GetMouseButtonDown(0) && _view.IsTouchInZone)
                _view.StartDrag();
            
            if (Input.GetMouseButton(0))
                _view.ProcessDrag();
            
            if (Input.GetMouseButtonUp(0))
                _view.EndDrag();
        }
    }
}