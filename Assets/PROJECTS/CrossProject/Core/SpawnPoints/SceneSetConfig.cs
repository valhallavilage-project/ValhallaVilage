using System.Collections.Generic;
using UnityEngine;

namespace CrossProject.Core.SpawnPoints
{
    [CreateAssetMenu(menuName = "Cross Project/SceneSetConfig", fileName = "SceneSetConfig")]
    public class SceneSetConfig : ScriptableObject
    {
        public List<string> items;
    }
}
