using Cysharp.Threading.Tasks;

namespace CrossProject.Core
{
    public abstract class BoxedValue<T>
    {
        protected readonly AsyncReactiveProperty<T> _maxValue = new(default);
        protected readonly AsyncReactiveProperty<T> _currentValue = new(default);
        protected readonly AsyncReactiveProperty<T> _minValue = new(default);
        
        public void Init(T max, T current, T min)
        {
            _maxValue.Value = max;
            _minValue.Value = min;
            _currentValue.Value = current;
        }

        protected void IncreaseCurrentValue(T value)
        {
            var currentValue = _currentValue.Value;
            
            _currentValue.Value = MinValue(Addition(currentValue, value), _maxValue.Value);
        }

        protected void ReduceCurrentValue(T value)
        {
            var currentValue = _currentValue.Value;
            _currentValue.Value = MaxValue(Subtraction(currentValue, value),_minValue.Value);
        }

        protected void IncreaseMaxValue(T value)
        {
            var maxValue = _maxValue.Value;
            _maxValue.Value = Addition(maxValue, value);
            IncreaseCurrentValue(value);
        }

        protected void ReduceMaxValue(T value)
        {
            var maxValue = _maxValue.Value;
            _maxValue.Value = MaxValue(Subtraction(maxValue, value),_minValue.Value);
            ReduceCurrentValue(value);
        }

        protected abstract T Subtraction(T minuend, T subtrahend);
        protected abstract T Addition(T augend, T addend);
        protected abstract T MaxValue(T compared, T comparator);
        protected abstract T MinValue(T compared, T comparator);
    }
}
