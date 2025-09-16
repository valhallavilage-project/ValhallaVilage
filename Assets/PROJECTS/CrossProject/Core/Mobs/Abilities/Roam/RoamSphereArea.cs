using UnityEngine;

namespace CrossProject.Core
{
    public class RoamSphereArea : IRoamArea
    {
        private SphereCollider _area;

        public float MinRoamPathLength { get; private set; }

        public void Init(Collider area, float minRoamPathLength)
        {
            _area = (SphereCollider)area;
            MinRoamPathLength = minRoamPathLength;
        }

        public bool IsInside(Vector3 position)
        {
            var center = _area.transform.position + _area.center;
            var radius = _area.radius * _area.transform.lossyScale.x;

            var distance = Vector3.Distance(position, center);

            return distance <= radius;
        }

        public Vector3 GetPointInside()
        {
            var randomPoint = Random.insideUnitCircle * _area.radius;
        
            return _area.transform.position + new Vector3(randomPoint.x, 0, randomPoint.y);
        }
    }
}
