using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace CrossProject.Core
{
    public interface IStateTimingHandler<in TTimingState>
        where TTimingState : Enum
    {
        void SetTiming(TTimingState timing, float duration);
        void RemoveTiming(TTimingState timing);
        bool IsTimePassed(TTimingState timing);
    }

    public abstract class BaseStateTimingHandler<TTimingState> : IStateTimingHandler<TTimingState>
        where TTimingState : Enum
    {
        private readonly float[] _timings;

        protected BaseStateTimingHandler()
        {
            _timings = new float[Enum.GetValues(typeof(TTimingState)).Length];
        }

        public void SetTiming(TTimingState timing, float duration)
        {
            var index = UnsafeUtility.As<TTimingState, int>(ref timing);

            _timings[index] = Time.time + duration;
        }

        public void RemoveTiming(TTimingState timing)
        {
            var index = UnsafeUtility.As<TTimingState, int>(ref timing);

            _timings[index] = 0;
        }

        public bool IsTimePassed(TTimingState timing)
        {
            var index = UnsafeUtility.As<TTimingState, int>(ref timing);

            return Time.time >= _timings[index];
        }
    }
}