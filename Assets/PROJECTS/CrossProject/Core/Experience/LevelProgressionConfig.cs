using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrossProject.Core
{
    [CreateAssetMenu(fileName = nameof(LevelProgressionConfig), menuName = "ScriptableObjects/Configs/LevelProgression")]
    public class LevelProgressionConfig : ScriptableObject
    {
        [SerializeField] [ListDrawerSettings(ShowIndexLabels = true)] private List<int> _progression;
        [SerializeField] private float _defaultExperienceRange;
        [SerializeField] private int _levelCap;

        public List<int> Progression => _progression;
        public float DefaultExperienceRange => _defaultExperienceRange;
        public int LevelCap => _levelCap;
    }
}
