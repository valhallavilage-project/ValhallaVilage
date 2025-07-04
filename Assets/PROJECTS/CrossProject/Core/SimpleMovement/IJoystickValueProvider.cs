using UnityEngine;

namespace CrossProject.Core.SimpleMovement
{
    //TODO : VM : Consider renaming to smth like IDirectionValueProvider
    public interface IJoystickValueProvider
    {
        Vector2 NormalizedVector2 { get; }
        Vector3 NormalizedVector3OnPlain { get; }
    }
}