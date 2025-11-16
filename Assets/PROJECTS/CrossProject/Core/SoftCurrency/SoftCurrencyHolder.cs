using CrossProject.Core.SaveLoad;
using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface ISoftCurrencyHolder
    {
        IReadOnlyAsyncReactiveProperty<int> AmountChanged { get; }
        
        int Amount { get; }
        void ChangeValue(int delta);
    }

    public class SoftCurrencyHolder : ISoftCurrencyHolder
    {
        private readonly AsyncReactiveProperty<int> _amountChanged = new(default);

        public IReadOnlyAsyncReactiveProperty<int> AmountChanged => _amountChanged;
        
        private readonly GameStateManager _gameStateManager;

        public SoftCurrencyHolder(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }

        public int Amount
        {
            get
            {
                var statePart = _gameStateManager.State.Get<SoftCurrencyStatePart>();

                return statePart.Amount;
            }
        }

        public void ChangeValue(int delta)
        {
            var statePart = _gameStateManager.State.Get<SoftCurrencyStatePart>();

            statePart.Amount += delta;
            
            _gameStateManager.Save();

            _amountChanged.Value = statePart.Amount;
        }
    }
}
