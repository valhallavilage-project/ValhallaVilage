using UnityEngine;

namespace CrossProject.Ui.Implementations
{
    [CreateAssetMenu(menuName = "Cross Project/Ui/Implementations/Joystick Config", fileName = nameof(JoystickConfig))]
    public class JoystickConfig : ScriptableObject
    {
        public Vector2 zoneAnchorMin;
        public Vector2 zoneAnchorMax;
        public float backgroundRadius;
        public float backgroundAlpha;
        public float stickRadius;
        public float stickAlpha;
        public float deadZoneRadius;
        public float maxZoneRadius;
    }
}