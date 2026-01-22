using System;
using System.Collections.Generic;
using System.Linq;
using CrossProject.Core.Characters;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace CrossProject.Core.Skins
{
    [CreateAssetMenu(menuName = "Cross Project/Skins/SkinSetConfig", fileName = "SkinSetConfig")]
    public class SkinSetConfig : ScriptableObject
    {
        public List<SkinConfig> items = new();

        public SkinConfig GetDefaultSkinFor(CharacterId characterId)
        {
            var defaultSkin = items.FirstOrDefault(x => x.owner == characterId && x.isDefaultSkin);
            if (defaultSkin == null)
            {
                Debug.LogWarning($"[SkinSetConfig] Default skin not found for: {characterId}, returning first skin");
                return items.FirstOrDefault(x => x.owner == characterId);
            }
            return defaultSkin;
        }
    }

    [Serializable]
    public class SkinConfig
    {
        [FoldoutGroup("$id")]
        public string id;

        [FoldoutGroup("$id")]
        [OdinSerialize]
        public CharacterId owner;

        [FoldoutGroup("$id")]
        public bool isDefaultSkin;
    }
}
