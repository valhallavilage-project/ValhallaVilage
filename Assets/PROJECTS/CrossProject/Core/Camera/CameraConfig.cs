using UnityEngine;

namespace CrossProject.Core.Camera
{
    [CreateAssetMenu(menuName = "Cross Project/Camera Config", fileName = nameof(CameraConfig))]
    public class CameraConfig : ScriptableObject
    {
        [Range(0, 360)]
        public int yRotationAngle;

        [Range(0, 360)]
        public int xRotationAngle;

        [Range(0, 100)]
        public int zoomDistance;
    }
}