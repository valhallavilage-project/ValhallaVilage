using System;
using AYellowpaper.SerializedCollections;
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
    }
    
    [Serializable]
    public class ArmorTypeToGameObjectDictionary : SerializedDictionary<MainCharacterArmorSetType, SetObject>
    {
    }

    public class Skin : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private ArmorTypeToGameObjectDictionary _armorSets;

        public Animator Animator => _animator;

        public void SelectSet(MainCharacterArmorSetType set)
        {
            foreach (var (_, setObject) in _armorSets)
            {
                setObject.SetActive(false);
            }
            
            _armorSets[set].SetActive(true);
        }
    }
}