using UnityEngine;

namespace L2Farm.Features.DayNight
{
    [CreateAssetMenu(menuName = "L2Farm/DayNight", fileName = "DayNightConfig")]
    public class DayNightConfig : ScriptableObject
    {
        public int updateIntervalInSeconds = 60;
        public Gradient gradient;
    }
}
