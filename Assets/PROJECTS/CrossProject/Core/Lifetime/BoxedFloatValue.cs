using System;

namespace CrossProject.Core
{
    public class BoxedFloatValue : BoxedValue<float>
    {
        protected void AssignMaxValue(float value)
        {
            var delta = value - _maxValue.Value;

            if (delta > 0)
            {
                IncreaseMaxValue(Math.Abs(delta));
            }
            else
            {
                ReduceMaxValue(Math.Abs(delta));
            }
        }
        
        protected override float Subtraction(float minuend, float subtrahend)
        {
            return minuend - subtrahend;
        }
        protected override float Addition(float augend, float addend)
        {
            return augend + addend;
        }
        protected override float MaxValue(float compared, float comparator)
        {
            return Math.Max(compared, comparator);
        }
        protected override float MinValue(float compared, float comparator)
        {
            return Math.Min(compared, comparator);
        }
    }
}
