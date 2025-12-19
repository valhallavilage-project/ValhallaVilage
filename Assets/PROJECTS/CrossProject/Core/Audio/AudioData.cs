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

        [Tooltip("Animation cycle duration in seconds. If > 0, sound will tick every X seconds (synced to animation loop)")]
        [SerializeField] [ShowIf(nameof(_isLoop))] private float _animationCycleDuration;

        public AudioClip Clip => _clip;
        public bool IsLoop => _isLoop;
        public bool IsStopPreviousClip => _isStopPreviousClip;
        public float AnimationCycleDuration => _animationCycleDuration;

        public bool IsEmpty => _clip == null;
        public bool IsSyncedToAnimation => _animationCycleDuration > 0;
    }
}
