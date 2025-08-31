using UnityEngine;

namespace CrossProject.Core.Experience
{
    [System.Serializable]
    public class SimpleExperienceProgression : IExperienceProgression
    {
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private int maxLevel = 99;
        [SerializeField] private int experiencePerLevel = 1000;

        public int StartingLevel => startingLevel;
        public int MaxLevel => maxLevel;

        public int ExperienceToNextLevel(int currentLevel, int currentExperiencePoints)
        {
            return experiencePerLevel - (currentExperiencePoints - currentLevel * experiencePerLevel);
        }
    }
}
