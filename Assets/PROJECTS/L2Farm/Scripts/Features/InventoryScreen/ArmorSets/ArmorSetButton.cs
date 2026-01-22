using System.Linq;
using CrossProject.Core;
using UnityEngine;
using VContainer;

namespace L2Farm
{
    public class ArmorSetButton : MonoBehaviour
    {
        [SerializeField] private MainCharacterArmorSetType _armorSet;
        [SerializeField] private MainCharacterArmorSetsConfig _armorSetConfigs;
        [SerializeField] private GameObject _unavailableImage;

        private IMainCharacterGlobalArmorSetChangeHandler _armorSetService;
        private MainCharacterSpecificArmorSetConfig _currentSetConfig;
        private IMainCharacterGlobalFacade _mainCharacterFacade;

        [Inject]
        private void AddDependencies(IMainCharacterGlobalArmorSetChangeHandler armorSetChangeHandler,
            IMainCharacterGlobalFacade mainCharacterFacade)
        {
            _mainCharacterFacade = mainCharacterFacade;
            _armorSetService = armorSetChangeHandler;

            _currentSetConfig = _armorSetConfigs.Configs.FirstOrDefault(c => c.ArmorSetType == _armorSet);
            if (_currentSetConfig == null)
            {
                Debug.LogError($"[ArmorSetButton] Armor set config not found: {_armorSet}");
                return;
            }
        }

        private void OnEnable()
        {
            _unavailableImage.SetActive(_currentSetConfig.Level > _mainCharacterFacade.CurrentLevel.Value);
        }

        public void ButtonPressed()
        {
            _armorSetService.ChangeSet(_armorSet);
        }
    }
}
