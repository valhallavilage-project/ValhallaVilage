using System;
using UnityEngine;

namespace CrossProject.Core
{
    [Serializable]
    public struct AnimationTransitionData<TStateType>
        where TStateType : Enum
    {
        [SerializeField] private TStateType _to;
        [SerializeField] private float _durationPercent;

        public TStateType To => _to;
        public float DurationPercent => _durationPercent;
    }
}
