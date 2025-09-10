using UnityEngine;

namespace CrossProject.Core
{
    public struct Velocity
    {
        public Vector3 Direction { get; }
        public float Speed { get; }

        public Vector3 Value { get; }

        public Velocity(Vector3 direction, float speed)
        {
            Direction = direction.normalized;
            Speed = speed;

            Value = direction * speed;
        }
    }
}