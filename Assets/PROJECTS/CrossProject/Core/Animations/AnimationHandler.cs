using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CrossProject.Core
{
    public interface IAnimationHandler<TStateType, TLayer>
        where TStateType : Enum
        where TLayer : Enum
    {
        void Init(IDictionary<TStateType, AnimationData<TStateType, TLayer>> statesToAnimation, Animator animator, float crossFadeTime);
        void StateChanged(TStateType state);
        void SetFloatParameter(string name, float value);
    }

    public abstract class AnimationHandler<TStateType, TLayer> : IAnimationHandler<TStateType, TLayer>
        where TStateType : Enum
        where TLayer : Enum
    {
        private IDictionary<TStateType, AnimationData<TStateType, TLayer>> _statesToAnimation;
        private Animator _animator;
        private float _crossFadeTime;
        private TStateType _currentState;

        private readonly HashSet<string> _triggers = new();

        public void Init(IDictionary<TStateType, AnimationData<TStateType, TLayer>> statesToAnimation, Animator animator, float crossFadeTime)
        {
            _statesToAnimation = statesToAnimation;
            _animator = animator;
            _crossFadeTime = crossFadeTime;
        }

        public void StateChanged(TStateType state)
        {
            foreach (var trigger in _triggers)
            {
                _animator.ResetTrigger(trigger);
            }

            var transitionTime = _crossFadeTime;

            if (_statesToAnimation.TryGetValue(_currentState, out var currentAnimationData))
            {
                if (currentAnimationData.Transitions is { Length: > 0 } && currentAnimationData.Transitions.Any(t => t.To.Equals(state)))
                {
                    transitionTime = currentAnimationData.Transitions.First(t => t.To.Equals(state)).DurationPercent;
                }
            }

            if (_statesToAnimation.TryGetValue(state, out var animationData))
            {
                if (animationData.SignalToLayer is { Length: > 0 })
                {
                    foreach (var layer in animationData.SignalToLayer)
                    {
                        var triggerName = $"{animationData.Layer}To{layer}";

                        _triggers.Add(triggerName);

                        _animator.SetTrigger(triggerName);
                    }
                }

                _animator.CrossFade($"{animationData.Layer}.{state.ToString()}", transitionTime);
            }

            _currentState = state;
        }

        public void SetFloatParameter(string name, float value)
        {
            _animator.SetFloat(name, value);
        }
    }
}
