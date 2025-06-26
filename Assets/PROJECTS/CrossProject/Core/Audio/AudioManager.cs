using UnityEngine;

namespace CrossProject.Core.Audio
{
    public class AudioManager : MonoBehaviour
    {
        private AudioSourcePool _pool;

        private void Awake()
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