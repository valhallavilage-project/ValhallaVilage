using CrossProject.Ui.Core;
using UnityEngine;

namespace CrossProject.Ui.Implementations
{
    public class JoystickModel : HudElementModel
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

        //TODO : VM : public static JoystickModel Default() {}

        public static JoystickModel From(JoystickConfig config)
        {
            return new JoystickModel
            {
                zoneAnchorMin = config.zoneAnchorMin,
                zoneAnchorMax = config.zoneAnchorMax,
                desiredPercentagePosition = config.desiredPercentagePosition,
                backgroundRadius = config.backgroundRadius,
                backgroundAlphaInactive = config.backgroundAlphaInactive,
                backgroundAlphaActive = config.backgroundAlphaActive,
                stickRadius = config.stickRadius,
                stickAlphaInactive = config.stickAlphaInactive,
                stickAlphaActive = config.stickAlphaActive,
                deadZoneRadius = config.deadZoneRadius,
                maxZoneRadius = config.maxZoneRadius,
                AssetOverride = config.assetOverride
            };
        }
    }
}