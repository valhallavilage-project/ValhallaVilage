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
            Play(audioData.Clip, audioData.IsLoop, audioData.IsStopPreviousClip);
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
            _audioSource.Stop();
        }
    }
}
