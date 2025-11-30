using UnityEngine;

namespace CrossProject.Core
{
    [CreateAssetMenu(fileName = nameof(GardenConfig), menuName = "ScriptableObjects/Configs/Garden")]
    public class GardenConfig : ScriptableObject
    {
        [SerializeField] private int _gardenBedClearEnergy;

        public int GardenBedClearEnergy => _gardenBedClearEnergy;
    }
}
