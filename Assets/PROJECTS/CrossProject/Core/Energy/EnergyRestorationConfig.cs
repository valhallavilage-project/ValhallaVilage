using UnityEngine;

namespace CrossProject.Core.Energy
{
    [CreateAssetMenu(menuName = "Cross Project/Energy Restoration Config", fileName = "EnergyRestorationConfig")]
    public class EnergyRestorationConfig : ScriptableObject
    {
        public int energyToRestoreForOneInterval = 1;
        public int intervalInSeconds = 60;
        public int energyMaximum = 100;
    }
}
