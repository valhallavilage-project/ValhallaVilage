using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CrossProject.Core;
using CrossProject.Core.InGameResources;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer.Unity;

namespace L2Farm
{
    public interface ITimersHandler
    {
        IReadOnlyAsyncReactiveProperty<string> TimerLaunched { get; }
        IReadOnlyAsyncReactiveProperty<string> TimerElapsed { get; }
        IReadOnlyAsyncReactiveProperty<string> TimerFinished { get; }

        string Launch(TimerSetupData timerSetupData);
        float GetTimeLeft(string timerId);
        TimerSetupData GetTimerData(string timerId);
        float ReduceTimerSeconds(float value);
    }

    public class TimerData
    {
        public TimerSetupData SetupData { get; set; }
        public DateTime StartTime { get; set; }
        public float TimeLeft { get; set; }
    }

    public class TimersHandler : ITimersHandler, IInitializable, ITickable
    {
        private readonly ITimeService _timeService;
        private readonly AsyncReactiveProperty<string> _timerLaunched = new(default);
        private readonly AsyncReactiveProperty<string> _timerElapsed = new(default);
        private readonly AsyncReactiveProperty<string> _timerFinished = new(default);
        private Dictionary<string, TimerData> _timers = new();
        private List<string> _finishedTimers = new();

        public IReadOnlyAsyncReactiveProperty<string> TimerLaunched => _timerLaunched;
        public IReadOnlyAsyncReactiveProperty<string> TimerElapsed => _timerElapsed;
        public IReadOnlyAsyncReactiveProperty<string> TimerFinished => _timerFinished;

        public bool IsInitialized { get; private set; }

        public TimersHandler(ITimeService timeService)
        {
            _timeService = timeService;
        }

        public async UniTask Initialize()
        {
            IsInitialized = true;
        }

        public string Launch(TimerSetupData timerSetupData)
        {
            var id = Guid.NewGuid().ToString();

            _timers.Add(id, new TimerData
            {
                SetupData = timerSetupData,
                StartTime = _timeService.Now,
                TimeLeft = timerSetupData.Seconds
            });

            _timerLaunched.Value = id;

            return id;
        }

        public float GetTimeLeft(string timerId)
        {
            if (!_timers.ContainsKey(timerId))
            {
                return -1;
            }

            return _timers[timerId].TimeLeft;
        }

        public TimerSetupData GetTimerData(string timerId)
        {
            return _timers[timerId].SetupData;
        }

        public float ReduceTimerSeconds(float value)
        {
            if (_timers.Values.Count == 0)
            {
                return value;
            }
            
            TimerData oldestTimer = null;
            var isFirst = true;

            foreach (var timer in _timers.Values)
            {
                if (timer.TimeLeft == 0)
                {
                    continue;
                }

                if (isFirst)
                {
                    isFirst = false;
                    oldestTimer = timer;

                    continue;
                }

                oldestTimer = timer.StartTime < oldestTimer.StartTime ? timer : oldestTimer;
            }

            if (oldestTimer == null)
            {
                return value;
            }

            var excessTime = Math.Max(value - oldestTimer.TimeLeft, 0);

            oldestTimer.TimeLeft = Mathf.Max(oldestTimer.TimeLeft - value, 0);

            return excessTime;
        }

        public void Tick()
        {
            _finishedTimers.Clear();

            foreach (var (timerId, timerData) in _timers)
            {
                timerData.TimeLeft = Mathf.Max(timerData.TimeLeft - Time.deltaTime, 0);

                if (timerData.TimeLeft == 0)
                {
                    _finishedTimers.Add(timerId);

                    ExecuteTimerCallbacks(timerData.SetupData.Callbacks).ContinueWith(() =>
                    {
                        _timerFinished.Value = timerId;
                    }).Forget();

                    _timerElapsed.Value = timerId;
                }
            }

            if (_finishedTimers.Count > 0)
            {
                foreach (var timerId in _finishedTimers)
                {
                    _timers.Remove(timerId);
                }
            }
        }

        private async UniTask ExecuteTimerCallbacks(IEnumerable<ITimerCallback> callbacks)
        {
            foreach (var callback in callbacks)
            {
                await callback.Execute();
            }
        }
    }
}
