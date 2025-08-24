using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace L2Farm.Features.DayNight
{
    public class DayNightService : MonoBehaviour , IDayNightProvider
    {
        [SerializeField] private Light sunlight;
        [SerializeField] private DayNightConfig config;

        private TimeSpan _interval;
        private CancellationTokenSource _cts = new ();

        public float Evaluation { get; private set; }
        public event Action<float> OnEvaluate;

        private void Start()
        {
            _interval = TimeSpan.FromSeconds(config.updateIntervalInSeconds);
            Routine(_cts.Token).Forget();
        }

        private async UniTask Routine(CancellationToken token)
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                    break;

                var now = DateTime.Now;
                var secondsSinceMidnight = (float)now.TimeOfDay.TotalSeconds;
                var halfDaySeconds = (float)TimeSpan.FromHours(12).TotalSeconds;
                Evaluation = secondsSinceMidnight <= halfDaySeconds
                    ? secondsSinceMidnight/halfDaySeconds
                    : 1 - (secondsSinceMidnight - halfDaySeconds)/halfDaySeconds;
                Debug.Log($"[{nameof(DayNightService)}] : {Evaluation}");
                OnEvaluate?.Invoke(Evaluation);
                sunlight.color = config.gradient.Evaluate(Evaluation);
                await UniTask.Delay(_interval);
            }
        }

        public void Set(float value)
        {
            _cts.Cancel();

            Evaluation = value;
            OnEvaluate?.Invoke(Evaluation);
            sunlight.color = config.gradient.Evaluate(Evaluation);
        }
    }
}
