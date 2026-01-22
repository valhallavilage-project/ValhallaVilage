using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CrossProject.Core
{
    [Serializable]
    public struct PotionInfo
    {
        [SerializeField] private PotionType _potionType;
        [SerializeField] private int _value;

        public PotionType PotionType => _potionType;
        public int Value => _value;
    }

    [CreateAssetMenu(fileName = nameof(PotionsConfig), menuName = "ScriptableObjects/Configs/Potions")]
    public class PotionsConfig : ScriptableObject
    {
        [SerializeField] private List<PotionInfo> _potions;

        public PotionInfo GetPotion(PotionType potionType)
        {
            var potion = _potions.FirstOrDefault(p => p.PotionType == potionType);
            if (potion.Equals(default(PotionInfo)))
            {
                Debug.LogError($"[PotionsConfig] Potion not found: {potionType}");
            }
            return potion;
        }
    }
}
