using UnityEngine;
using CrossProject.Core.Pooling;

namespace CrossProject.Core.Audio{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourcePoolElement : MonoPoolElement
    {
        public AudioSource Source { get; private set; }

        protected override void OnAwake()
        {
            Source = GetComponent<AudioSource>();
        }

        public override void OnReturn()
        {
            Source.Stop();
            Source.clip = null;
            Source.pitch = 1;
            Source.loop = false;
            base.OnReturn();
        }

        public override bool IsAvailableToGet => !Source.isPlaying;
    }
}