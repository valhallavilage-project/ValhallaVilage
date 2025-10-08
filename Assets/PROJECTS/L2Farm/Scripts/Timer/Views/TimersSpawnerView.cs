using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer;

namespace L2Farm
{
    public class TimersSpawnerView : MonoBehaviour
    {
        [SerializeField] private TimersPool _timersPool;

        private ITimersHandler _timersHandler;

        private readonly Dictionary<string, TimerView> _timers = new();

        [Inject]
        private void AddDependencies(ITimersHandler timersHandler)
        {
            _timersHandler = timersHandler;

            timersHandler.TimerLaunched.WithoutCurrent().ForEachAsync(TimerLaunched, gameObject.GetCancellationTokenOnDestroy()).Forget();
            timersHandler.TimerFinished.WithoutCurrent().ForEachAsync(TimerFinished, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void TimerLaunched(string timerId)
        {
            var data = _timersHandler.GetTimerData(timerId);

            var timerView = _timersPool.Get();

            timerView.transform.position = data.WorldPosition;
            timerView.Setup(timerId);
            
            _timers.Add(timerId, timerView);
        }

        private void TimerFinished(string timerId)
        {
            var timer = _timers[timerId];
            
            _timersPool.Return(timer);
        }
    }
}
