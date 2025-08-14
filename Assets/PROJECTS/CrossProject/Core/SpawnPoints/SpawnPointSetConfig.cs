using System.Collections.Generic;
using UnityEngine;

namespace CrossProject.Core.SpawnPoints
{
    [CreateAssetMenu(menuName = "Cross Project/SpawnPointSetConfig", fileName = "SpawnPointSetConfig")]
    public class SpawnPointSetConfig : ScriptableObject
    {
        public List<SpawnPointConfig> items = new();
    }
}
