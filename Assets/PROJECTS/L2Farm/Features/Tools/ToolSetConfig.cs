using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace L2Farm.Features.Tools
{
    [CreateAssetMenu(menuName = "L2Farm/ToolSetConfig", fileName = "ToolSetConfig")]
    public class ToolSetConfig : ScriptableObject
    {
        public List<ToolConfig> items = new();
    }

    [System.Serializable]
    public class ToolConfig
    {
        [FoldoutGroup("$id")]
        public string id;

        [FoldoutGroup("$id")]
        public Mesh mesh;

        [FoldoutGroup("$id")]
        public Material material;
    }
}
