using System;
using UnityEngine;

namespace CrossProject.Core
{
    public interface IMobPersistentData
    {
        Vector3 RoamPathStart { get; set; }
        Vector3 RoamPathDestination { get; set; }
        Vector3 RoamPathDirection { get; set; }
        bool IsRoamMoveOnPathFinished { get; set; }
        bool IsRoamRotationFinished { get; set; }
        bool IsRotationToTargetFinished { get; set; }
        bool IsTargetApproached { get; set; }
    }
    
    public class MobPersistentData : IMobPersistentData
    {
        public Vector3 RoamPathStart { get; set; }
        public Vector3 RoamPathDestination { get; set; }
        public Vector3 RoamPathDirection { get; set; }
        public bool IsRoamMoveOnPathFinished { get; set; }
        public bool IsRoamRotationFinished { get; set; }
        public bool IsRotationToTargetFinished { get; set; }
        public bool IsTargetApproached { get; set; }
    }
}