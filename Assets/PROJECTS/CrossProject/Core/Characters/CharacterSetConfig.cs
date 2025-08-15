using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrossProject.Core.Characters
{
    [CreateAssetMenu(menuName = "Cross Project/Characters/CharacterSetConfig", fileName = "CharacterSetConfig")]
    public class CharacterSetConfig : ScriptableObject
    {
        public List<CharacterConfig> items = new();
    }

    [Serializable]
    public class CharacterConfig
    {
        [FoldoutGroup("$id")]
        public string id;

        [FoldoutGroup("$id")]
        [PreviewField(128f, ObjectFieldAlignment.Right)]
        public Sprite portrait;

        [FoldoutGroup("$id")]
        public string personName;
    }
}