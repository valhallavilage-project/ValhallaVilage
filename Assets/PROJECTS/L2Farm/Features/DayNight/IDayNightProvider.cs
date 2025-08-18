using System;

namespace L2Farm.Features.DayNight
{
    public interface IDayNightProvider
    {
        float Evaluation { get; }
        event Action<float> OnEvaluate;
    }
}
