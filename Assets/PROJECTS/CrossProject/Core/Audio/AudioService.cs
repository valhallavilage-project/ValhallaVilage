using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public interface IAudioService
    {
        void Init(AudioSource audioSource);
        void Play(AudioClip clip, bool isLoop, bool isStopPreviousClip = false);
        void Stop();
        void Play(AudioData audioData);
    }

    public class AudioService : IAudioService
    {
        private readonly IAudioSettings _audioSettings;
        private AudioSource _audioSource;
        private CancellationTokenSource _animationSyncCts;

        public AudioService(IAudioSettings audioSettings)
        {
            _audioSettings = audioSettings;
        }
        
        public void Init(AudioSource audioSource)
        {
            _audioSource = audioSource;
        }

        public void Play(AudioData audioData)
        {
            Debug.Log($"[AudioService] Play called: clip={audioData.Clip?.name ?? "null"}, isLoop={audioData.IsLoop}, isSynced={audioData.IsSyncedToAnimation}, cycleDuration={audioData.AnimationCycleDuration}");

            // If sound is synced to animation cycle - use ticking logic
            if (audioData.IsSyncedToAnimation)
            {
                // Stop any previous loop sound first
                _audioSource.Stop();
                _audioSource.clip = null;

                PlaySyncedToAnimation(audioData).Forget();
            }
            else
            {
                Play(audioData.Clip, audioData.IsLoop, audioData.IsStopPreviousClip);
            }
        }

        private async UniTask PlaySyncedToAnimation(AudioData audioData)
        {
            // IMPORTANT: Cancel previous animation-synced sound FIRST to prevent multiple loops (echo)
            if (_animationSyncCts != null)
            {
                Debug.Log("[AudioService] Cancelling previous synced sound loop to prevent echo");
                _animationSyncCts.Cancel();
                _animationSyncCts.Dispose();
                // Wait a frame for cancellation to complete
                await UniTask.Yield();
            }

            _animationSyncCts = new CancellationTokenSource();

            Debug.Log($"[AudioService] Starting NEW synced sound loop, cycleDuration={audioData.AnimationCycleDuration}s");

            try
            {
                // Play sound in loop synced to animation cycle duration
                while (!_animationSyncCts.Token.IsCancellationRequested)
                {
                    if (audioData.Clip != null)
                    {
                        Debug.Log($"[AudioService] Playing tick: {audioData.Clip.name}");
                        _audioSource.PlayOneShot(audioData.Clip, _audioSettings.SoundVolume);
                    }

                    // Wait for animation cycle to complete before next tick
                    await UniTask.Delay(System.TimeSpan.FromSeconds(audioData.AnimationCycleDuration),
                        cancellationToken: _animationSyncCts.Token);
                }
            }
            catch (System.OperationCanceledException)
            {
                Debug.Log("[AudioService] Synced sound cancelled (normal stop)");
            }
        }

        public void Play(AudioClip clip, bool isLoop, bool isStopPreviousClip = false)
        {
            if (isStopPreviousClip)
            {
                _audioSource.Stop();
                _audioSource.clip = null;
            }

            if (clip == null)
            {
                return;
            }
            
            if (isLoop)
            {
                if (_audioSource.clip != clip)
                {
                    _audioSource.volume = _audioSettings.SoundVolume;
                    _audioSource.clip = clip;
                    _audioSource.loop = isLoop;
                    _audioSource.Play();
                }
            }
            else
            {
                _audioSource.PlayOneShot(clip);
            }
        }

        public void Stop()
        {
            Debug.Log("[AudioService] Stop called");

            // Cancel animation-synced sound loop
            _animationSyncCts?.Cancel();
            _animationSyncCts?.Dispose();
            _animationSyncCts = null;

            _audioSource.Stop();
            _audioSource.clip = null;
        }
    }
}
