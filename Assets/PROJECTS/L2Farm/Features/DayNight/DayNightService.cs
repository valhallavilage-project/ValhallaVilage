using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace L2Farm.Features.DayNight
{
    public class DayNightService : MonoBehaviour , IDayNightProvider
    {
        [SerializeField] private Light sunlight;
        [SerializeField] private DayNightConfig config;

        private TimeSpan interval;

        public float Evaluation { get; private set; }
        public event Action<float> OnEvaluate;

        private void Start()
        {
            interval = TimeSpan.FromSeconds(config.updateIntervalInSeconds);
            Routine().Forget();
        }

        private async UniTask Routine()
        {
            while (true)
            {
                var now = DateTime.Now;
                var secondsSinceMidnight = (float)now.TimeOfDay.TotalSeconds;
                var halfDaySeconds = (float)TimeSpan.FromHours(12).TotalSeconds;
                Evaluation = secondsSinceMidnight <= halfDaySeconds
                    ? secondsSinceMidnight/halfDaySeconds
                    : 1 - (secondsSinceMidnight - halfDaySeconds)/halfDaySeconds;
                Debug.Log($"[{nameof(DayNightService)}] : {Evaluation}");
                OnEvaluate?.Invoke(Evaluation);
                sunlight.color = config.gradient.Evaluate(Evaluation);
                await UniTask.Delay(interval);
            }
        }
    }
}
