using System;
using System.Collections.Generic;
using CrossProject.Core.Skins;
using UnityEngine;

namespace CrossProject.Core.Characters
{
    [CreateAssetMenu(menuName = "Cross Project/Characters/CharacterSetConfig", fileName = "CharacterSetConfig")]
    public class CharacterSetConfig : ScriptableObject
    {
        public List<CharacterConfig> items = new ();

        public CharacterId GetOwnerOf(SkinId skinId)
        {
            foreach (var characterConfig in items)
                if (characterConfig.defaultSkinId == skinId || characterConfig.otherSkinIds.Contains(skinId))
                    return characterConfig.id;

            throw new Exception($"There is no CharacterId configured for : {skinId}!");
        }
    }

    [Serializable]
    public class CharacterConfig
    {
        public CharacterId id;
        public SkinId defaultSkinId;
        public List<SkinId> otherSkinIds = new ();
    }
}