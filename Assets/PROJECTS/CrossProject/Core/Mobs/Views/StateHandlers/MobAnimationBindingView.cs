using System;
using AYellowpaper.SerializedCollections;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    [Serializable]
    public class MobStateToAnimationDataDictionary : SerializedDictionary<MobState, AnimationData<MobState, MobAnimationLayer>>
    {
    }

    public class MobAnimationBindingView : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private float _crossFadeTime;
        [SerializeField] private MobStateToAnimationDataDictionary _data;

        private IMobAnimationHandler _animationHandler;

        [Inject]
        private void AddDependencies(IMobStateMachine stateMachine, IMobAnimationHandler animationHandler)
        {
            _animationHandler = animationHandler;
            
            animationHandler.Init(_data, _animator, _crossFadeTime);

            stateMachine.CurrentState.ForEachAsync(StateChanged, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void StateChanged(MobState nextState)
        {
            _animationHandler.StateChanged(nextState);
        }
    }
}
