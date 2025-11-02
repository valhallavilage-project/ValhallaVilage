using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CrossProject.Core
{
    [Serializable]
    public struct AudioData
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private bool _isLoop;
        [SerializeField] [HideIf(nameof(_isLoop))] private bool _isStopPreviousClip;

        public AudioClip Clip => _clip;
        public bool IsLoop => _isLoop;
        public bool IsStopPreviousClip => _isStopPreviousClip;

        public bool IsEmpty => _clip == null;
    }
}
