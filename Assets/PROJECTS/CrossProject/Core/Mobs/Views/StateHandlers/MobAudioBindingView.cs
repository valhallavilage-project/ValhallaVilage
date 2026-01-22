using System;
using AYellowpaper.SerializedCollections;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Sirenix.OdinInspector;
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
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private MobStateToAudioDataDictionary _data;
        
        [FoldoutGroup("AdditionalSounds")][SerializeField] private AudioData _receiveDamage;
        
        private IAudioService _audioService;

        [Inject]
        private void AddDependencies(IMobStateMachine stateMachine, IDamageReceiveHandler damageReceiveHandler, IAudioService audioService)
        {
            _audioService = audioService;
            
            audioService.Init(_audioSource);
            
            stateMachine.CurrentState.WithoutCurrent().ForEachAsync(StateChanged, gameObject.GetCancellationTokenOnDestroy()).Forget();
            damageReceiveHandler.DamageReceived.WithoutCurrent().ForEachAsync(DamageReceived, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void DamageReceived(float _)
        {
            if (_receiveDamage.Clip != null)
            {
                _audioService.Play(_receiveDamage);
            }
        }

        private void StateChanged(MobState nextState)
        {
            if (!_data.ContainsKey(nextState))
            {
                _audioService.Stop();
                return;
            }

            var data = _data[nextState];

            if (data.IsEmpty)
            {
                _audioService.Stop();
                return;
            }

            // Skip looped sounds (footsteps) - they cause audio chaos when many mobs are nearby
            // Keep only one-shot sounds like attacks, death, etc.
            if (data.IsLoop)
            {
                return;
            }

            _audioService.Play(data);
        }
    }
}
