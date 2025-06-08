using System.Collections.Generic;
using UnityEngine;

namespace RUNNER.Scripts.Core.Levels
{
    [CreateAssetMenu(fileName = nameof(LevelConfiguration), menuName = "Watchtower/LevelConfiguration}")]
    public class LevelConfiguration : ScriptableObject
    {
        public List<GameObject> levels = new ();
    }
}