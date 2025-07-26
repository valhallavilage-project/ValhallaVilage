using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrossProject.Core.PROJECTS.CrossProject.Core.InGameResources
{
    [CreateAssetMenu(menuName = "Cross Project/InGameResources/ResourceSetConfig", fileName = "ResourceSetConfig")]
    public class ResourceSetConfig : ScriptableObject
    {
        public List<ResourceConfig> items = new();

        private void OnValidate()
        {
            foreach (var config in items)
                if (string.IsNullOrEmpty(config.id) && config.icon != null)
                    config.id = config.icon.name;
        }
    }

    [Serializable]
    public class ResourceConfig
    {
        [FoldoutGroup("$id")]
        public string id;

        [FoldoutGroup("$id")]
        [PreviewField(128, ObjectFieldAlignment.Right)]
        public Sprite icon;

        [FoldoutGroup("$id")]
        public bool isUnique;

        [FoldoutGroup("$id")]
        [HideIf("$isUnique")]
        public int maxStackCapacity = 64;
    }
}
