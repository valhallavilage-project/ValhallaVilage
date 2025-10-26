using UnityEngine;

namespace CrossProject.Core
{
    [CreateAssetMenu(fileName = nameof(MainCharacterSpecificArmorSetConfig), menuName = "ScriptableObjects/Configs/MainCharacterArmorSetConfig")]
    public class MainCharacterSpecificArmorSetConfig : ScriptableObject
    {
        [SerializeField] private MainCharacterArmorSetType _armorSetType;
        [SerializeField] private int _health;
        [SerializeField] private int _energy;
        [SerializeField] private int _damage;
        [SerializeField] private int _levelAvailable;

        public MainCharacterArmorSetType ArmorSetType => _armorSetType;
        public int Health => _health;
        public int Energy => _energy;
        public int Damage => _damage;
        public int Level => _levelAvailable;
    }
}
