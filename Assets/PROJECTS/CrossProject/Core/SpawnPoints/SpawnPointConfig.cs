using Sirenix.OdinInspector;
using UnityEngine;

namespace CrossProject.Core.SpawnPoints
{
    [System.Serializable]
    public class SpawnPointConfig
    {
        [FoldoutGroup("$id")] public string id;
        [FoldoutGroup("$id")] public SceneId sceneId;
        [FoldoutGroup("$id")] public Vector3 position;
        [FoldoutGroup("$id")] public Vector3 eulerAngles;
    }
}
