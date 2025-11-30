using Cysharp.Threading.Tasks;

namespace L2Farm
{
    public interface IGlobalActivateGardenHandler
    {
        IReadOnlyAsyncReactiveProperty<Invoker> GardenBedsActivated { get; }
        
        void ActivateGardenBeds();
    }

    public class GlobalActivateGardenHandler : IGlobalActivateGardenHandler
    {
        private readonly AsyncReactiveProperty<Invoker> _gardenBedsActivated = new(default);

        public IReadOnlyAsyncReactiveProperty<Invoker> GardenBedsActivated => _gardenBedsActivated;

        public void ActivateGardenBeds()
        {
            _gardenBedsActivated.Invoke();
        }
    }
}
