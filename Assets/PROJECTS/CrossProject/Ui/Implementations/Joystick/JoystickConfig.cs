using UnityEngine;

namespace CrossProject.Ui.Implementations
{
    [CreateAssetMenu(menuName = "Cross Project/Ui/Implementations/Joystick Config", fileName = nameof(JoystickConfig))]
    public class JoystickConfig : ScriptableObject
    {
        public Vector2 zoneAnchorMin;
        public Vector2 zoneAnchorMax;
        public Vector2 desiredPercentagePosition;
        public float backgroundRadius;
        public float backgroundAlphaInactive;
        public float backgroundAlphaActive;
        public float stickRadius;
        public float stickAlphaInactive;
        public float stickAlphaActive;
        public float deadZoneRadius;
        public float maxZoneRadius;
        public string assetOverride;
    }
}