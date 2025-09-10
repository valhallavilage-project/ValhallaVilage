using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrossProject.Core.Quests
{
    [CreateAssetMenu(menuName = "L2Farm/IndicationTypeSetConfig", fileName = "IndicationTypeSetConfig")]
    public class IndicationTypeSetConfig : ScriptableObject
    {
        public List<IndicationTypeConfig> items = new();
    }

    [System.Serializable]
    public class IndicationTypeConfig
    {
        [FoldoutGroup("$id")]
        public string id;

        [FoldoutGroup("$id")]
        [PreviewField(128, ObjectFieldAlignment.Left)]
        public Sprite icon;

        [FoldoutGroup("$id")]
        public Color color = Color.white;
    }
}
