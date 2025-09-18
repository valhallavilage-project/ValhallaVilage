using UnityEngine;

namespace CrossProject.Core.Audio
{
    public interface IAudioHandler
    {
        void PlayOneShot(AudioClip clip, float volume = 1f, float pitch = 1f, Transform parent = null);
        void PlayBGM(AudioClip clip = null);
        void ToggleSFX();
        void ToggleBGM();
    }

    public class AudioManager : MonoBehaviour, IAudioHandler
    {
        [SerializeField]
        private AudioSource _bgm;

        [SerializeField]
        private AudioSourcePool _pool;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void RepositionAudioSource(Transform source, Transform parent)
        {
            source.SetParent(parent);
            source.localPosition = Vector3.zero;
        }

        public void PlayOneShot(AudioClip clip, float volume = 1f, float pitch = 1f, Transform parent = null)
        {
            var element = _pool.Get();
            if (parent != null)
                RepositionAudioSource(element.transform, parent);

            element.Source.pitch = pitch;
            element.Source.PlayOneShot(clip, volume);
        }

        public void PlayBGM(AudioClip clip = null)
        {
            if (clip != null)
                _bgm.clip = clip;
            _bgm.Play();
        }

        public void ToggleSFX()
        {
            _pool.ToggleMute();
        }

        public void ToggleBGM()
        {
            _bgm.mute = !_bgm.mute;
        }
    }
}