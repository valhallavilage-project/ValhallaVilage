using UnityEngine;

namespace L2Farm.Features.DayNight
{
    public class EnvironmentLighting : AbstractDayNightListener
    {
        [SerializeField] private float baseIntensity = 10;
        [SerializeField] private Light pointLight;
        [SerializeField] private GameObject vfx;

        protected override void Evaluate(float evaluation)
        {
            int mode = 1 - Mathf.RoundToInt(evaluation);
            //Debug.Log($"[{nameof(EnvironmentLighting)}] Result : {mode} from Evaluation {evaluation};");
            pointLight.intensity = baseIntensity * mode;
            vfx?.SetActive(mode > 0);
        }
    }
}
