using CrossProject.Core;
using UnityEngine;
using VContainer;

namespace L2Farm.Features.DayNight
{
    public abstract class AbstractDayNightListener : MonoBehaviour
    {
        protected IDayNightProvider dayNightProvider;

        private void Start()
        {
            Injector.Instance?.Inject(this);
        }

        [Inject]
        private void Construct(IDayNightProvider provider)
        {
            dayNightProvider = provider;
            dayNightProvider.OnEvaluate += Evaluate;
        }

        protected abstract void Evaluate(float evaluation);
    }
}
