using System;
using System.Collections.Generic;
using CrossProject.Core.Conditions;
using CrossProject.Core.SpawnPoints;
using Sirenix.OdinInspector;
using UnityEngine;

namespace L2Farm.Features.Buildings
{
    [CreateAssetMenu(menuName = "L2Farm/BuildingSetConfig", fileName = "BuildingSetConfig")]
    public class BuildingSetConfig : ScriptableObject
    {
        public List<BuildingConfig> items = new();
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
    }
}
