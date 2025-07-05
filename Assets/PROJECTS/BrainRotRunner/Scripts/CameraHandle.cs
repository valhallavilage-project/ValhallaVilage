using System;
using System.Collections.Generic;
using CrossProject.Core;
using UnityEngine;

namespace RUNNER.Scripts
{
    public class CameraHandle : CustomBehaviour, IBlockable
    {
        [SerializeField] private Transform target;
        [SerializeField] private bool followY = true;
        [SerializeField] private Transform rotationHandle;
        [SerializeField] private Vector2 rotationRange = new (0, 360);
        [SerializeField] private float rotationSpeed = 4;
        [SerializeField] private float rotationDeadZone = 0.1f;
        [SerializeField] private Transform zoomHandle;
        [SerializeField] private Vector2 zoomRange = new(0, 100);
        [SerializeField] private float zoomSpeed = 4;
        [SerializeField] private float zoomDeadZone = 0.1f;
        [SerializeField] private Transform verticalAngleHandle;
        [SerializeField] private Vector2 verticalAngleRange = new(-90, 90);
        [SerializeField] private float verticalAngleSpeed = 4;
        [SerializeField] private float verticalAngleDeadZone = 0.1f;

        private float _rotate;
        private float _zoom;
        private float _verticalAngle;

        private readonly HashSet<Type> _blockers = new();

        private void FollowTarget()
        {
            if (target == null)
                return;

            var position = target.position;
            if (!followY)
                position.y = transform.position.y;
            transform.position = position;
        }

        private void GetInput()
        {
            _rotate = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
            _zoom = Input.GetAxis("Vertical") * zoomSpeed * Time.deltaTime;
            _verticalAngle = Input.GetKey(KeyCode.Mouse1)
                ? Input.GetAxis("Vertical") * verticalAngleSpeed * Time.deltaTime
                : 0;
        }

        private void Rotate()
        {
            rotationHandle.Rotate(Vector3.up, _rotate, Space.Self);
            float yRotation = Mathf.Clamp(rotationHandle.eulerAngles.y, rotationRange.x, rotationRange.y);
            rotationHandle.localRotation = Quaternion.Euler(0, yRotation, 0);
        }

        private void Zoom()
        {
            var targetPosition = zoomHandle.localPosition + _zoom * Vector3.up;
            targetPosition.y = Mathf.Clamp(targetPosition.y, zoomRange.x, zoomRange.y);
            zoomHandle.localPosition = targetPosition;
        }

        private void ChangeVerticalAngle()
        {
            verticalAngleHandle.Rotate(Vector3.right, _verticalAngle, Space.Self);
            float yRotation = Mathf.Clamp(verticalAngleHandle.eulerAngles.y, verticalAngleRange.x, verticalAngleRange.y);
            verticalAngleHandle.localRotation = Quaternion.Euler(0, yRotation, 0);
        }

        protected override void OnUpdate()
        {
            FollowTarget();

            if (IsBlocked)
                return;

            GetInput();

            if (Mathf.Abs(_rotate) > rotationDeadZone)
                Rotate();

            if (Mathf.Abs(_zoom) > zoomDeadZone)
                Zoom();

            if (Mathf.Abs(_verticalAngle) > verticalAngleDeadZone)
                ChangeVerticalAngle();
        }

        public void AddBlock(object blockRequester)
        {
            _blockers.Add(blockRequester.GetType());
        }

        public void RemoveBlock(object blockRequester)
        {
            _blockers.Remove(blockRequester.GetType());
        }

        public bool IsBlocked => _blockers.Count > 0;
    }
}