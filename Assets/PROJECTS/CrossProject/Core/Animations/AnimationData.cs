using System;
using UnityEngine;

namespace CrossProject.Core
{
    [Serializable]
    public struct AnimationData<TStateType, TLayer> 
        where TStateType: Enum
        where TLayer: Enum
    {
        [SerializeField] private TLayer _layer;
        [SerializeField] private TLayer[] _signalToLayer;
        [SerializeField] private AnimationTransitionData<TStateType>[] _transitions;

        public TLayer Layer => _layer;
        public AnimationTransitionData<TStateType>[] Transitions => _transitions;
        public TLayer[] SignalToLayer => _signalToLayer;
    }
}
