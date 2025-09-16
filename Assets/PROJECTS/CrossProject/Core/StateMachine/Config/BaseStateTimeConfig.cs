using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace CrossProject.Core
{
    public abstract class BaseStateTimeConfig<TStateType, TStateToTimeDictionary> : ScriptableObject
        where TStateType : Enum
        where TStateToTimeDictionary : SerializedDictionary<TStateType, float>
    {
        [SerializeField, SerializedDictionary("State", "Time")] private TStateToTimeDictionary _stateTime;

        public float GetDelay(TStateType state)
        {
            return _stateTime.ContainsKey(state) ? _stateTime[state] : 0;
        }
    }
}