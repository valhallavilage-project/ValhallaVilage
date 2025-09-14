using UnityEngine;

namespace CrossProject.Core
{
    [CreateAssetMenu(menuName = "Cross Project/Energy Restoration Config", fileName = "EnergyRestorationConfig")]
    public class EnergyRestorationConfig : ScriptableObject
    {
        [SerializeField] private float _energyToRestoreForOneInterval = 1;
        [SerializeField] private int _intervalInSeconds = 60;

        public float EnergyToRestoreForOneInterval => _energyToRestoreForOneInterval;
        public int IntervalInSeconds => _intervalInSeconds;
    }
}
