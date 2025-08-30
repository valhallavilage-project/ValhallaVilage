using System;
using System.Collections.Generic;
using System.Linq;
using CrossProject.Core.Conditions;
using CrossProject.Core.Quests;
using CrossProject.Core.SpawnPoints;
using Sirenix.OdinInspector;
using UnityEngine;

namespace L2Farm.Features.Buildings
{
    [CreateAssetMenu(menuName = "L2Farm/BuildingSetConfig", fileName = "BuildingSetConfig")]
    public class BuildingSetConfig : ScriptableObject
    {
        public List<BuildingConfig> items = new();

        public int GetSecondsFor(BuildingId buildingId) => items.First(x => x.id == buildingId).timeToBuildInSeconds;

        public QuestId GetQuestFor(BuildingId buildingId) => items.First(x => x.id == buildingId).questToLaunchOnComplete;
    }

    [Serializable]
    public class BuildingConfig
    {
        [FoldoutGroup("$id")]
        public string id;

        [FoldoutGroup("$id")]
        public SceneId sceneId;

        [FoldoutGroup("$id")]
        public SpawnPointId spawnPointId;

        [FoldoutGroup("$id")]
        public string assetIdBroken = "_Broken";

        [FoldoutGroup("$id")]
        public string assetIdReady = "_Ready";

        [FoldoutGroup("$id")]
        [SerializeReference]
        public IConditionConfig spawnReadyCondition;

        [FoldoutGroup("$id")]
        public float buildingVFXScale = 1;

        [FoldoutGroup("$id")]
        public Vector3 buildingVFXOffset = Vector3.zero;

        [FoldoutGroup("$id")]
        public int timeToBuildInSeconds = 60;

        [FoldoutGroup("$id")]
        public QuestId questToLaunchOnComplete;
    }
}
