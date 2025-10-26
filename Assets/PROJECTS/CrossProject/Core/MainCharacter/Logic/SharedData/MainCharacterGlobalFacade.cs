using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IMainCharacterGlobalFacade
    {
        AsyncReactiveProperty<float> CurrentHealth { get; }
        AsyncReactiveProperty<float> MaxHealth { get; }
        AsyncReactiveProperty<float> CurrentEnergy { get; }
        AsyncReactiveProperty<float> MaxEnergy { get; }
        AsyncReactiveProperty<float> CurrentExperience { get; }
        AsyncReactiveProperty<float> MaxExperience { get; }
        AsyncReactiveProperty<float> MinExperience { get; }
        AsyncReactiveProperty<int> CurrentLevel { get; }
        AsyncReactiveProperty<bool> IsDied { get; }
    }

    public class MainCharacterGlobalFacade : IMainCharacterGlobalFacade
    {
        public AsyncReactiveProperty<float> CurrentHealth { get; } = new(default);
        public AsyncReactiveProperty<float> MaxHealth { get; } = new(default);
        public AsyncReactiveProperty<float> CurrentEnergy { get; } = new(default);
        public AsyncReactiveProperty<float> MaxEnergy { get; } = new(default);
        public AsyncReactiveProperty<float> CurrentExperience { get; } = new(default);
        public AsyncReactiveProperty<float> MinExperience { get; } = new(default);
        public AsyncReactiveProperty<float> MaxExperience { get; } = new(default);
        public AsyncReactiveProperty<int> CurrentLevel { get; } = new(default);
        public AsyncReactiveProperty<bool> IsDied { get; } = new(default);
    }
}
