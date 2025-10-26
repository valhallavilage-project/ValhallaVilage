using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public interface IBoxedValueHandler<in T>
    {
        bool IsInitialized { get; }

        void Init(T max, T current, T min);
    }

    public abstract class BoxedValue<T> : IBoxedValueHandler<T>
    {
        protected readonly AsyncReactiveProperty<T> _maxValue = new(default);
        protected readonly AsyncReactiveProperty<T> _currentValue = new(default);
        protected readonly AsyncReactiveProperty<T> _minValue = new(default);

        public bool IsInitialized { get; private set; }

        public void Init(T max, T current, T min)
        {
            _maxValue.Value = max;
            _minValue.Value = min;
            _currentValue.Value = current;

            IsInitialized = true;
        }

        protected void IncreaseCurrentValue(T value)
        {
            var currentValue = _currentValue.Value;

            _currentValue.Value = MinValue(Addition(currentValue, value), _maxValue.Value);
        }

        protected void ReduceCurrentValue(T value)
        {
            var currentValue = _currentValue.Value;
            _currentValue.Value = MaxValue(Subtraction(currentValue, value), _minValue.Value);
        }

        protected void IncreaseMaxValue(T value)
        {
            var maxValue = _maxValue.Value;
            _maxValue.Value = Addition(maxValue, value);
        }

        protected void ReduceMaxValue(T value)
        {
            var maxValue = _maxValue.Value;
            _maxValue.Value = MaxValue(Subtraction(maxValue, value), _minValue.Value);
        }

        protected abstract T Subtraction(T minuend, T subtrahend);
        protected abstract T Addition(T augend, T addend);
        protected abstract T MaxValue(T compared, T comparator);
        protected abstract T MinValue(T compared, T comparator);
    }
}
