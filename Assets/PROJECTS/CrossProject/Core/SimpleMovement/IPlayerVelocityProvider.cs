using UnityEngine;

namespace CrossProject.Core.SimpleMovement
{
    public interface IPlayerVelocityProvider
    {
        Vector3 Velocity { get; }
        Vector3 Direction { get; }
    }
}