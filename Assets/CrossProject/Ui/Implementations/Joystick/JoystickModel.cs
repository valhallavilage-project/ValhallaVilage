using CrossProject.Ui.Core;
using UnityEngine;

namespace CrossProject.Ui.Implementations
{
    public class JoystickModel : HudElementModel
    {
        public Vector2 zoneAnchorMin;
        public Vector2 zoneAnchorMax;
        public float backgroundRadius;
        public float backgroundAlpha;
        public float stickRadius;
        public float stickAlpha;
        public float deadZoneRadius;
        public float maxZoneRadius;

        //TODO : VM : public static JoystickModel Default() {}

        public static JoystickModel From(JoystickConfig config)
        {
            return new JoystickModel
            {
                zoneAnchorMin = config.zoneAnchorMin,
                zoneAnchorMax = config.zoneAnchorMax,
                backgroundRadius = config.backgroundRadius,
                backgroundAlpha = config.backgroundAlpha,
                stickRadius = config.stickRadius,
                stickAlpha = config.stickAlpha,
                deadZoneRadius = config.deadZoneRadius,
                maxZoneRadius = config.maxZoneRadius
            };
        }
    }
}