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
        private int _currentLoopId;

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
                var oldLoopId = _currentLoopId;

                // Cancel any existing animation-synced loop first
                _animationSyncCts?.Cancel();
                _animationSyncCts?.Dispose();

                // Increment loop ID to invalidate any running loops
                _currentLoopId++;

                // Stop AudioSource to prevent overlap
                _audioSource.Stop();
                _audioSource.clip = null;

                // Create new CTS for new loop
                _animationSyncCts = new CancellationTokenSource();

                PlaySyncedToAnimation(audioData, _currentLoopId).Forget();
            }
            else
            {
                Play(audioData.Clip, audioData.IsLoop, audioData.IsStopPreviousClip);
            }
        }

        private async UniTask PlaySyncedToAnimation(AudioData audioData, int loopId)
        {
            var token = _animationSyncCts?.Token ?? default;

            if (token == default || token.IsCancellationRequested)
            {
                return;
            }

            try
            {
                while (!token.IsCancellationRequested && _currentLoopId == loopId)
                {
                    if (_currentLoopId != loopId)
                    {
                        return;
                    }

                    if (audioData.Clip != null)
                    {
                        _audioSource.PlayOneShot(audioData.Clip, _audioSettings.SoundVolume);
                    }

                    await UniTask.Delay(System.TimeSpan.FromSeconds(audioData.AnimationCycleDuration),
                        cancellationToken: token);
                }
            }
            catch (System.OperationCanceledException)
            {
                // Expected when sound is stopped
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
            _currentLoopId++;

            // Cancel animation-synced sound loop
            _animationSyncCts?.Cancel();
            _animationSyncCts?.Dispose();
            _animationSyncCts = null;

            _audioSource.Stop();
            _audioSource.clip = null;
        }
    }
}
