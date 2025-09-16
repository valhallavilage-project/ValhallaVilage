using UnityEngine;

namespace CrossProject.Core
{
    [CreateAssetMenu(fileName = nameof(MainCharacterClothesSetConfig), menuName = "ScriptableObjects/Configs/MainCharacterSetConfig")]
    public class MainCharacterClothesSetConfig : ScriptableObject
    {
        [SerializeField] private MainCharacterClothesSetType _clothesSetType;
        [SerializeField] private int _health;
        [SerializeField] private int _energy;
        [SerializeField] private int _damage;

        public MainCharacterClothesSetType ClothesSetType => _clothesSetType;
        public int Health => _health;
        public int Energy => _energy;
        public int Damage => _damage;
    }
}
