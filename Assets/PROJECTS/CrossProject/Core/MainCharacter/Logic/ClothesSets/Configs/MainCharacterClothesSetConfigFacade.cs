using UnityEngine;

namespace CrossProject.Core
{
    [CreateAssetMenu(fileName = nameof(MainCharacterClothesSetConfigFacade), menuName = "ScriptableObjects/Configs/MainCharacterClothesSetConfigFacade")]
    public class MainCharacterClothesSetConfigFacade : ScriptableObject
    {
        [SerializeField] private MainCharacterClothesSetConfig[] _configs;

        public MainCharacterClothesSetConfig[] Configs => _configs;
    }
}
