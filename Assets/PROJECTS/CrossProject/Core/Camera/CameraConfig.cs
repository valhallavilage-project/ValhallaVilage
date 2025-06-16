using UnityEngine;

namespace CrossProject.Core.Camera
{
    [CreateAssetMenu(menuName = "Cross Project/Camera Config", fileName = nameof(CameraConfig))]
    public class CameraConfig : ScriptableObject
    {
        public int yRotationAngle;
        public int xRotationAngle;
        public int zoomDistance;
    }
}