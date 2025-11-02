using System.Collections.Generic;
using CrossProject.Core;
using UnityEngine;

namespace L2Farm
{
    public class TimerSetupData
    {
        public float Seconds { get; set; }
        public float VfxScale { get; set; } = 1;
        public Vector3 WorldPosition { get; set; }
        public AudioData SoundFx { get; set; }

        public List<ITimerCallback> Callbacks { get; } = new();
    }
}
