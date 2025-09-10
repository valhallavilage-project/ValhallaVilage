using System.Collections.Generic;
using CrossProject.Core.InGameResources;
using CrossProject.Core.Quests;
using L2Farm.Features.Buildings;
using Sirenix.OdinInspector;
using UnityEngine;

namespace L2Farm.Features.ResourceProduction
{
    [CreateAssetMenu(menuName = "L2Farm/ProductionSetConfig", fileName = "ProductionSetConfig")]
    public class ProductionSetConfig : ScriptableObject
    {
        public List<ProductionConfig> productionConfigs = new();
    }

    [System.Serializable]
    public class ProductionConfig
    {
        [FoldoutGroup("$id")]
        public string id;

        [FoldoutGroup("$id")]
        public int timeToProduceInSeconds = 60;

        [FoldoutGroup("$id")]
        public BuildingId buildingId;

        [FoldoutGroup("$id")]
        public QuestId finishQuest;
    }
}
