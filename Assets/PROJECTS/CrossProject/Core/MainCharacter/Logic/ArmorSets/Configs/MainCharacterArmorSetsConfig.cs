using UnityEngine;

namespace CrossProject.Core
{
    [CreateAssetMenu(fileName = nameof(MainCharacterArmorSetsConfig), menuName = "ScriptableObjects/Configs/MainCharacterArmorSetConfigFacade")]
    public class MainCharacterArmorSetsConfig : ScriptableObject
    {
        [SerializeField] private MainCharacterSpecificArmorSetConfig[] _configs;

        public MainCharacterSpecificArmorSetConfig[] Configs => _configs;
    }
}
