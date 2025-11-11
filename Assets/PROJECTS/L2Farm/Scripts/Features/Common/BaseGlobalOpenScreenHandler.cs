using Cysharp.Threading.Tasks;

namespace L2Farm
{
    public interface IGlobalOpenScreenHandler
    {
        IReadOnlyAsyncReactiveProperty<Invoker> ScreenOpenRequested { get; }
        void OpenScreen();
    }

    public abstract class BaseGlobalOpenScreenHandler : IGlobalOpenScreenHandler
    {
        private readonly AsyncReactiveProperty<Invoker> _screenOpenRequested = new(default);

        public IReadOnlyAsyncReactiveProperty<Invoker> ScreenOpenRequested => _screenOpenRequested;

        public void OpenScreen()
        {
            _screenOpenRequested.Invoke();
        }
    }
}
