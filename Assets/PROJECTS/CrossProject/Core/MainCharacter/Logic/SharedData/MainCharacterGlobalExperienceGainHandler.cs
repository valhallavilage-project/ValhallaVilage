using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IMainCharacterGlobalExperienceGainHandler
    {
        IReadOnlyAsyncReactiveProperty<float> ExperienceGained { get; }
        void GainXp(float experience);
    }

    public class MainCharacterGlobalExperienceGainHandler : IMainCharacterGlobalExperienceGainHandler
    {
        private readonly AsyncReactiveProperty<float> _experienceGained = new(default);

        public IReadOnlyAsyncReactiveProperty<float> ExperienceGained => _experienceGained;

        public void GainXp(float experience)
        {
            _experienceGained.Value = experience;
        }
    }
}
