using System;
using System.Collections.Generic;
using CrossProject.Core.Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace L2Farm.Features.NPC
{
    [CreateAssetMenu(menuName = "L2Farm/NPCSetConfig", fileName = "NPCSetConfig")]
    public class NPCSetConfig : ScriptableObject
    {
        public List<NPCConfig> items = new ();
    }

    [Serializable]
    public class NPCConfig
    {
        [FoldoutGroup("$id")]
        public string id;

        [FoldoutGroup("$id")]
        public CharacterId characterId;

        [FoldoutGroup("$id")]
        [PreviewField(128f, ObjectFieldAlignment.Right)]
        public GameObject prefab;
    }
}
