using UnityEngine;

namespace CrossProject.Core.SpawnPoints
{
    [System.Serializable]
    public class SpawnPointConfig
    {
        public string id;
        public SceneId sceneId;
        public Vector3 position;
        public Vector3 eulerAngles;
    }
}
