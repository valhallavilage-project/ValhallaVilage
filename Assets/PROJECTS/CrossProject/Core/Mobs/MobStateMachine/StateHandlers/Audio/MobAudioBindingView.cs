using System;
using AYellowpaper.SerializedCollections;
using CrossProject.Core.Audio;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    [Serializable]
    public class MobStateToAudioDataDictionary : SerializedDictionary<MobState, AudioData>
    {
    }
    
    public class MobAudioBindingView : MonoBehaviour
    {
        [SerializeField] private MobStateToAudioDataDictionary _data;
        
        private IAudioHandler _audioHandler;

        [Inject]
        private void AddDependencies(IMobStateMachine stateMachine, IAudioHandler audioHandler)
        {
            _audioHandler = audioHandler;
            
            stateMachine.CurrentState.ForEachAsync(StateChanged, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void StateChanged(MobState nextState)
        {
            if (!_data.ContainsKey(nextState))
            {
                return;
            }
            
            var data = _data[nextState];
            
            _audioHandler.PlayOneShot(data.Clip, data.Volume, data.Pitch, transform);
        }
    }
}
