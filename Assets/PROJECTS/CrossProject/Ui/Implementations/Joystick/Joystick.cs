using CrossProject.Ui.Core;
using UnityEngine;

namespace CrossProject.Ui.Implementations
{
    public class Joystick : HudElementView<JoystickModel>
    {
        [SerializeField] private RectTransform zone;
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform stick;

        private CanvasGroup _backgroundCanvasGroup;
        private CanvasGroup _stickCanvasGroup;
        private float _sqrMaxZoneRadius;
        private float _sqrDeadZoneRadius;

        public bool IsTouchInZone => Input.mousePosition.y <= Screen.height * zone.anchorMax.y &&
                                     Input.mousePosition.x <= Screen.width * zone.anchorMax.x;

        //TODO : add smooth value between max and dead zone
        public Vector2 NormalizedValue { get; private set; }

        protected override void OnBind()
        {
            zone.anchorMin = Model.zoneAnchorMin;
            zone.anchorMax = Model.zoneAnchorMax;
            zone.anchoredPosition = Vector2.zero;
            zone.sizeDelta = Vector2.zero;

            background.anchoredPosition = Vector2.zero;
            background.sizeDelta = new Vector2(Model.backgroundRadius, Model.backgroundRadius);
            if (background.TryGetComponent(out _backgroundCanvasGroup))
                _backgroundCanvasGroup.alpha = Model.backgroundAlphaInactive;

            stick.sizeDelta = new Vector2(Model.stickRadius, Model.stickRadius);
            if (stick.TryGetComponent(out _stickCanvasGroup))
                _stickCanvasGroup.alpha = Model.stickAlphaInactive;

            _sqrDeadZoneRadius = Model.deadZoneRadius * Model.deadZoneRadius;
            _sqrMaxZoneRadius = Model.maxZoneRadius * Model.maxZoneRadius;
        }

        public void StartDrag()
        {
            background.anchorMin = background.anchorMax = new Vector2(0.5f, 0.5f);
            background.position = Input.mousePosition;
            _backgroundCanvasGroup.alpha = Model.backgroundAlphaActive;
            _stickCanvasGroup.alpha = Model.stickAlphaActive;
        }

        public void ProcessDrag()
        {
            stick.position = Input.mousePosition;
            var sqrMagnitude = stick.anchoredPosition.sqrMagnitude;
            if (sqrMagnitude > _sqrMaxZoneRadius)
                stick.anchoredPosition = stick.anchoredPosition.normalized * Model.maxZoneRadius;
            //TODO : add smooth value between max and dead zone
            NormalizedValue = sqrMagnitude > _sqrDeadZoneRadius
                ? stick.anchoredPosition.normalized
                : Vector2.zero;
        }

        public void EndDrag()
        {
            var parentRect = transform.parent.GetComponent<RectTransform>().sizeDelta;
            background.anchorMin = background.anchorMax = Vector2.zero;
            var desiredPosition =
                new Vector2(
                    parentRect.x * Model.desiredPercentagePosition.x,
                    parentRect.y * Model.desiredPercentagePosition.y);
            background.anchoredPosition = desiredPosition;
            _backgroundCanvasGroup.alpha = Model.backgroundAlphaInactive;
            stick.anchoredPosition = Vector3.zero;
            _stickCanvasGroup.alpha = Model.stickAlphaInactive;
            NormalizedValue = Vector2.zero;
        }
    }
}