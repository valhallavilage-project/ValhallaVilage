using UnityEngine;

namespace L2Farm.Features.DayNight
{
    public class EnvironmentLighting : AbstractDayNightListener
    {
        [SerializeField] private float baseIntensity = 10;
        [SerializeField] private Light pointLight;

        protected override void Evaluate(float evaluation)
        {
            pointLight.intensity = baseIntensity * Mathf.RoundToInt(evaluation);
        }
    }
}
