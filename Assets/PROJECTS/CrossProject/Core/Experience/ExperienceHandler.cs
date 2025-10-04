using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public interface IExperienceHandler : IBoxedValueHandler<float>
    {
        IReadOnlyAsyncReactiveProperty<int> CurrentLevel { get; }
        IReadOnlyAsyncReactiveProperty<float> CurrentExperience { get; }
        IReadOnlyAsyncReactiveProperty<float> MinExperience { get; }
        IReadOnlyAsyncReactiveProperty<float> MaxExperience { get; }

        void Init(float currentXp, int currentLevel);
        void GainXp(float value);
    }

    public class ExperienceHandler : BoxedFloatValue, IExperienceHandler
    {
        private readonly LevelProgressionConfig _levelProgressionConfig;
        private readonly AsyncReactiveProperty<int> _currentLevel = new(default);

        public IReadOnlyAsyncReactiveProperty<int> CurrentLevel => _currentLevel;
        public IReadOnlyAsyncReactiveProperty<float> CurrentExperience => _currentValue;
        public IReadOnlyAsyncReactiveProperty<float> MinExperience => _minValue;
        public IReadOnlyAsyncReactiveProperty<float> MaxExperience => _maxValue;

        public ExperienceHandler(LevelProgressionConfig levelProgressionConfig)
        {
            _levelProgressionConfig = levelProgressionConfig;
        }

        public void Init(float currentXp, int currentLevel)
        {
            _currentLevel.Value = currentLevel;

            var maxXpValue = 0;
            var minXpValue = 0;

            if (currentLevel < _levelProgressionConfig.Progression.Count)
            {
                maxXpValue = _levelProgressionConfig.Progression[currentLevel];
                minXpValue = _levelProgressionConfig.Progression[currentLevel - 1];
            }
            else
            {
                var lastKnownLevelValue = _levelProgressionConfig.Progression[^1];

                var lastKnownLevel = _levelProgressionConfig.Progression.Count;

                for (var level = lastKnownLevel + 1; level <= currentLevel; level++)
                {
                    minXpValue = lastKnownLevelValue;
                    maxXpValue = minXpValue + _levelProgressionConfig.DefaultExperienceRange;

                    lastKnownLevelValue = maxXpValue;
                }
            }

            base.Init(maxXpValue, currentXp, minXpValue);
        }

        public void GainXp(float value)
        {
            var xpRemains = 0f;

            if (_currentValue.Value + value > _maxValue.Value)
            {
                xpRemains = _currentValue.Value + value - _maxValue.Value;
            }

            IncreaseCurrentValue(value);

            if (Math.Abs(_currentValue.Value - _maxValue.Value) < float.Epsilon)
            {
                GainNextLevel();
            }

            if (xpRemains > 0)
            {
                GainXp(xpRemains);
            }
        }

        private void GainNextLevel()
        {
            var nextLevel = _currentLevel.Value + 1;

            if (nextLevel > _levelProgressionConfig.LevelCap)
            {
                return;
            }

            _minValue.Value = _maxValue.Value;

            if (nextLevel < _levelProgressionConfig.Progression.Count)
            {
                _maxValue.Value = _levelProgressionConfig.Progression[nextLevel];
            }
            else
            {
                _maxValue.Value += _levelProgressionConfig.DefaultExperienceRange;
            }

            _currentLevel.Value = nextLevel;
        }
    }
}
