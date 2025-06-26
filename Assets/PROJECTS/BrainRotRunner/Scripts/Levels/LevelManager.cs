using CrossProject.Core;
using CrossProject.Core.SaveLoad;
using UnityEngine;

namespace RUNNER.Scripts.Core.Levels
{
    public class LevelManager
    {
        private int _currentLevel;

        [SerializeField] private LevelConfiguration levelConfiguration;

        private void Awake()
        {
            LoadLevel(_currentLevel);
        }

        private int GetNextLevelIndex()
        {
            return levelConfiguration.levels.Count < _currentLevel
                ? _currentLevel
                : _currentLevel % levelConfiguration.levels.Count;
        }

        private void LoadLevel(int levelNumber)
        {
            
        }

        public void LaunchLoadedLevel()
        {
            
        }

        public void CompleteCurrentLevel()
        {
            //GameStateManager.Instance.Save();
            //TODO : Effects
            //TODO : Try Show Interstitial
            //TODO : Destroy current level
        }
    }
}