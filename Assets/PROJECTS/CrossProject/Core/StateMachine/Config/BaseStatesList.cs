using System;
using System.Collections.Generic;
using UnityEngine;

namespace CrossProject.Core
{
    [Serializable]
    public abstract class BaseStatesList<TStateType>
        where TStateType : Enum
    {
        [SerializeField] private List<TStateType> _usedInStates = new();

        public List<TStateType> UsedInStates
        {
            get => _usedInStates;
            set => _usedInStates = value;
        }
    }
}