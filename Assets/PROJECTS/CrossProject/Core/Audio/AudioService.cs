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
        private readonly object _syncLock = new object();

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
            // If sound is synced to animation cycle - use ticking logic
            if (audioData.IsSyncedToAnimation)
            {
                // CRITICAL: Stop previous loop AFTER canceling CTS to prevent race condition
                lock (_syncLock)
                {
                    // Cancel any existing animation-synced loop first
                    _animationSyncCts?.Cancel();
                    _animationSyncCts?.Dispose();
                    _animationSyncCts = new CancellationTokenSource();
                }

                // Stop AudioSource AFTER canceling token - this ensures old loop is stopped
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
            // Capture the token inside the lock to ensure we're using the correct CTS
            CancellationToken token;
            lock (_syncLock)
            {
                if (_animationSyncCts == null || _animationSyncCts.IsCancellationRequested)
                {
                    return; // Already cancelled
                }
                token = _animationSyncCts.Token;
            }

            try
            {
                // Play sound in loop synced to animation cycle duration
                // Use local token variable - _animationSyncCts may be replaced by another call
                while (!token.IsCancellationRequested)
                {
                    if (audioData.Clip != null)
                    {
                        _audioSource.PlayOneShot(audioData.Clip, _audioSettings.SoundVolume);
                    }

                    // Wait for animation cycle to complete before next tick
                    await UniTask.Delay(System.TimeSpan.FromSeconds(audioData.AnimationCycleDuration),
                        cancellationToken: token);
                }
            }
            catch (System.OperationCanceledException)
            {
                // Normal cancellation when stopping movement/interaction
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
            // Cancel animation-synced sound loop
            lock (_syncLock)
            {
                _animationSyncCts?.Cancel();
                _animationSyncCts?.Dispose();
                _animationSyncCts = null;
            }

            _audioSource.Stop();
            _audioSource.clip = null;
        }
    }
}
