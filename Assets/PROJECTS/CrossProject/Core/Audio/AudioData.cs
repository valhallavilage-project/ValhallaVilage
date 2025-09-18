using System;
using UnityEngine;

namespace CrossProject.Core
{
    [Serializable]
    public struct AudioData
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private float _volume;
        [SerializeField] private float _pitch;

        public AudioClip Clip => _clip;
        public float Volume => _volume;
        public float Pitch => _pitch;
    }
}
