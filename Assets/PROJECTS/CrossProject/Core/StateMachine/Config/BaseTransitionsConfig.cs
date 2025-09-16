using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrossProject.Core
{
    public interface ITransitionsConfig<TStateType, TTransitionType>
        where TStateType : Enum
        where TTransitionType : Enum
    {
        IEnumerable<TTransitionType> GetTransitions(TStateType state);
        bool ContainsState(TStateType state);
        bool ContainsTransition(TTransitionType transition);
        List<TStateType> GetStatesForTransition(TTransitionType transition);
        bool CanTransitionInterruptState(TStateType currentState, TTransitionType interruptingTransition);
    }

    public abstract class BaseTransitionsConfig<TStateType, TTransitionType, TStatesList, TTransitionsList, TStateToTransitionsDictionary,
        TTransitionToStateDictionary> :
        ScriptableObject, ITransitionsConfig<TStateType, TTransitionType>
        where TStateType : Enum
        where TTransitionType : Enum
        where TStatesList : BaseStatesList<TStateType>, new()
        where TTransitionsList : BaseTransitionsList<TTransitionType>, new()
        where TStateToTransitionsDictionary : SerializedDictionary<TStateType, TTransitionsList>
        where TTransitionToStateDictionary : SerializedDictionary<TTransitionType, TStatesList>
    {
        [SerializeField, SerializedDictionary("State", "Transitions"), OnValueChanged("SyncTransitionsToStateCache")] private TStateToTransitionsDictionary _states;
        [SerializeField, SerializedDictionary("State", "Transitions")] private TStateToTransitionsDictionary _stateInterruptedByTransitions;
        [SerializeField, SerializedDictionary("Transition", "State"), HideInInspector] private TTransitionToStateDictionary _transitions;

        [SerializeField] private List<TStateType> _statesForAdd;
        [SerializeField] private TTransitionType _transitionForAdd;
        [SerializeField] private TTransitionType _transitionForRemove;

        public IEnumerable<TTransitionType> GetTransitions(TStateType state)
        {
            return _states[state].UsedTransitions;
        }

        public bool ContainsState(TStateType state)
        {
            return _states.ContainsKey(state);
        }

        public bool ContainsTransition(TTransitionType transition)
        {
            return _transitions.ContainsKey(transition);
        }

        public List<TStateType> GetStatesForTransition(TTransitionType transition)
        {
            return _transitions[transition].UsedInStates;
        }

        public bool CanTransitionInterruptState(TStateType currentState, TTransitionType interruptingTransition)
        {
            return _stateInterruptedByTransitions.ContainsKey(currentState) && _stateInterruptedByTransitions[currentState].UsedTransitions.Contains(interruptingTransition);
        }

        [Button]
        public void RemoveTransitionFromAllStates()
        {
            if (_transitionForRemove == null)
            {
                return;
            }

            foreach (var stateKeyValue in _states)
            {
                stateKeyValue.Value.RemoveTransition(_transitionForRemove);
            }

            SyncTransitionsToStateCache();
            _transitionForRemove = default;
        }

        [Button]
        public void ClearStates()
        {
            _statesForAdd.Clear();
        }

        [Button]
        public void AddTransitionToStates()
        {
            foreach (var characterState in _statesForAdd)
            {
                if (!_states.ContainsKey(characterState))
                {
                    _states.Add(characterState, new TTransitionsList());
                }

                _states[characterState].AddTransition(_transitionForAdd);
            }

            SyncTransitionsToStateCache();

            _statesForAdd.Clear();
            _transitionForAdd = default;
        }

        [Button]
        public void SyncTransitionsToStateCache()
        {
            _transitions.Clear();

            foreach (var state in _states)
            {
                foreach (var transition in state.Value.UsedTransitions)
                {
                    if (!_transitions.ContainsKey(transition))
                    {
                        var list = new TStatesList
                        {
                            UsedInStates = new List<TStateType>()
                        };

                        _transitions.Add(transition, list);
                    }

                    _transitions[transition].UsedInStates.Add(state.Key);
                }
            }
        }
    }
}