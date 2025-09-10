using System;
using System.Collections.Generic;
using UnityEngine;

namespace CrossProject.Core
{
    [Serializable]
    public abstract class BaseTransitionsList<TTransitionState>
        where TTransitionState : Enum
    {
        [SerializeField] private List<TTransitionState> _usedTransitions = new();

        public List<TTransitionState> UsedTransitions
        {
            get => _usedTransitions;
            set => _usedTransitions = value;
        }

        public void AddTransition(TTransitionState transition)
        {
            if (_usedTransitions.Contains(transition))
            {
                return;
            }

            _usedTransitions.Add(transition);
        }

        public void RemoveTransition(TTransitionState transition)
        {
            _usedTransitions.Remove(transition);
        }
    }
}