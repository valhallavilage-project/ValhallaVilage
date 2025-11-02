using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IMainCharacterMovingHandler
    {
        IReadOnlyAsyncReactiveProperty<Invoker> MoveBegan { get; }
        IReadOnlyAsyncReactiveProperty<Invoker> MoveStopped { get; }
        void BeginMove();
        void StopMove();
    }

    public class MainCharacterMovingHandler : IMainCharacterMovingHandler
    {
        private readonly AsyncReactiveProperty<Invoker> _moveBegan = new(default);
        private readonly AsyncReactiveProperty<Invoker> _moveStopped = new(default);

        public IReadOnlyAsyncReactiveProperty<Invoker> MoveBegan => _moveBegan;
        public IReadOnlyAsyncReactiveProperty<Invoker> MoveStopped => _moveStopped;

        public void BeginMove()
        {
            _moveBegan.Invoke();
        }

        public void StopMove()
        {
            _moveStopped.Invoke();
        }
    }
}
