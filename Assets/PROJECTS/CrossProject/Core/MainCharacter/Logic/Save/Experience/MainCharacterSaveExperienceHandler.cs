using System;
using System.Threading;
using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using VContainer.Unity;

namespace CrossProject.Core
{
    public class MainCharacterSaveExperienceHandler : IInitializable, IDisposable
    {
        private readonly IRestoreEnergyHandler _restoreEnergyHandler;
        private readonly IExperienceHandler _experienceHandler;
        private readonly GameStateManager _gameStateManager;
        private readonly CancellationTokenSource _disposeCts = new();

        public bool IsInitialized { get; private set; }

        public MainCharacterSaveExperienceHandler(IExperienceHandler experienceHandler, GameStateManager gameStateManager)
        {
            _experienceHandler = experienceHandler;
            _gameStateManager = gameStateManager;
        }

        public UniTask Initialize()
        {
            if (!_gameStateManager.State.TryGet<ExperienceStatePart>(out var statePart))
            {
                statePart = new ExperienceStatePart
                {
                    Experience = 0,
                    Level = 1
                };

                _gameStateManager.State.Set(statePart);
                _gameStateManager.Save();
            }

            _experienceHandler.Init(statePart.Experience, statePart.Level);
            
            _experienceHandler.CurrentExperience.WithoutCurrent().ForEachAsync(ExperienceChanged, _disposeCts.Token).Forget();
            _experienceHandler.CurrentLevel.WithoutCurrent().ForEachAsync(LevelChanged, _disposeCts.Token).Forget();

            IsInitialized = true;

            return UniTask.CompletedTask;
        }

        private void LevelChanged(int level)
        {
            var statePart =_gameStateManager.State.Get<ExperienceStatePart>();

            statePart.Level = level;

            _gameStateManager.Save();
        }

        private void ExperienceChanged(float newValue)
        {
            var statePart =_gameStateManager.State.Get<ExperienceStatePart>();

            statePart.Experience = newValue;

            _gameStateManager.Save();
        }

        public void Dispose()
        {
            _disposeCts?.Dispose();
        }
    }
}
