using System;
using System.Collections.Generic;
using System.Linq;
using CrossProject.Core.Characters;
using Sirenix.Serialization;
using UnityEngine;

namespace CrossProject.Core.Skins
{
    [CreateAssetMenu(menuName = "Cross Project/Skins/SkinSetConfig", fileName = "SkinSetConfig")]
    public class SkinSetConfig : ScriptableObject
    {
        public List<SkinConfig> items = new();

        public SkinConfig GetDefaultSkinFor(CharacterId characterId)
            => items.First(x => x.owner == characterId && x.isDefaultSkin);
    }

    [Serializable]
    public class SkinConfig
    {
        public string id;
        [OdinSerialize] public CharacterId owner;
        public Skin skin;
        public bool isDefaultSkin;
    }
}
