using System;
using AYellowpaper.SerializedCollections;
using CrossProject.Core.Interactions;
using UnityEngine;

namespace CrossProject.Core.Skins
{
    [Serializable]
    public struct SetObject
    {
        [SerializeField] private GameObject _armorMesh;
        [SerializeField] private GameObject _weaponMesh;

        public void SetActive(bool isActive)
        {
            _armorMesh.SetActive(isActive);
            _weaponMesh.SetActive(isActive);
        }

        public void DeactivateWeapon()
        {
            _weaponMesh.SetActive(false);
        }
    }

    [Serializable]
    public class ArmorTypeToGameObjectDictionary : SerializedDictionary<MainCharacterArmorSetType, SetObject>
    {
    }

    public class Skin : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ArmorTypeToGameObjectDictionary _armorSets;
        [SerializeField] private GameObject _axeMesh;
        [SerializeField] private GameObject _pickAxeMesh;

        private MainCharacterArmorSetType _currentArmorSet;

        public Animator Animator => _animator;

        public void SelectSet(MainCharacterArmorSetType set)
        {
            _currentArmorSet = set;

            foreach (var (_, setObject) in _armorSets)
            {
                setObject.SetActive(false);
            }

            _armorSets[set].SetActive(true);
        }

        public void ActivateTool(InteractionType interaction)
        {
            _armorSets[_currentArmorSet].DeactivateWeapon();

            switch (interaction)
            {
                case InteractionType.Chop:
                    if (_axeMesh != null)
                    {
                        _axeMesh.SetActive(true);
                    }

                    break;
                case InteractionType.Pickaxe:
                    if (_pickAxeMesh != null)
                    {
                        _pickAxeMesh.SetActive(true);
                    }

                    break;
            }
        }

        public void DeactivateTool()
        {
            _armorSets[_currentArmorSet].SetActive(true);

            if (_axeMesh != null)
            {
                _axeMesh.SetActive(false);
            }

            if (_pickAxeMesh != null)
            {
                _pickAxeMesh.SetActive(false);
            }
        }
    }
}
