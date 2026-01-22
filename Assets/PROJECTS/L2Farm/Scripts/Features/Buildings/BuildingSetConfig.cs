using System;
using System.Collections.Generic;
using System.Linq;
using CrossProject.Core;
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

        public int GetSecondsFor(BuildingId buildingId)
        {
            var building = items.FirstOrDefault(x => x.id == buildingId);
            return building?.timeToBuildInSeconds ?? 60;
        }

        public QuestId GetQuestFor(BuildingId buildingId)
        {
            var building = items.FirstOrDefault(x => x.id == buildingId);
            return building?.questToLaunchOnComplete;
        }
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

        [FoldoutGroup("$id")]
        public float completeBuildingXpReward = 400;

        [FoldoutGroup("$id")]
        public AudioData buildingSound;
    }
}
