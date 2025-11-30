using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IMainCharacterGlobalCleanGardenBedHandler
    {
        IReadOnlyAsyncReactiveProperty<Invoker> GardenBedCleared { get; }
        void ClearGardenBed();
    }

    public class MainCharacterGlobalCleanGardenBedHandler : IMainCharacterGlobalCleanGardenBedHandler
    {
        private readonly AsyncReactiveProperty<Invoker> _gardenBedCleared = new(default);

        public IReadOnlyAsyncReactiveProperty<Invoker> GardenBedCleared => _gardenBedCleared;

        public void ClearGardenBed()
        {
            _gardenBedCleared.Invoke();
        }
    }
}
