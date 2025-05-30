using UnityEngine;

namespace CrossProject.Core.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoSingleton<AudioManager>
    {
        private AudioSourcePool _pool;

        protected override void OnAwake()
        {
            var poolPrefab = Resources.Load<AudioSourcePool>($"Pools/{nameof(AudioSourcePool)}");
            _pool = Instantiate(poolPrefab);
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

        public void PlayLooped(AudioClip clip, Transform parent = null)
        {
            var element = _pool.Get();
            if (parent != null)
                RepositionAudioSource(element.transform, parent);

            element.Source.loop = true;
            element.Source.clip = clip;
            element.Source.Play();
        }
    }
}