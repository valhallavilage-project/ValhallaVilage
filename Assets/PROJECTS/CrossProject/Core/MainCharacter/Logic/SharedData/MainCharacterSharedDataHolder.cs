using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IMainCharacterSharedDataHolder
    {
        AsyncReactiveProperty<float> CurrentHealth { get; }
        AsyncReactiveProperty<float> MaxHealth { get; }
        AsyncReactiveProperty<float> CurrentEnergy { get; }
        AsyncReactiveProperty<float> MaxEnergy { get; }
    }

    public class MainCharacterSharedDataHolder : IMainCharacterSharedDataHolder
    {
        public AsyncReactiveProperty<float> CurrentHealth { get; } = new(default);
        public AsyncReactiveProperty<float> MaxHealth { get; } = new(default);
        public AsyncReactiveProperty<float> CurrentEnergy { get; } = new(default);
        public AsyncReactiveProperty<float> MaxEnergy { get; } = new(default);
    }
}
