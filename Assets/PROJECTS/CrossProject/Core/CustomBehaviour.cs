using UnityEngine;

namespace CrossProject.Core
{
    public abstract class CustomBehaviour : MonoBehaviour
    {
        protected virtual void OnAwake() { }
        protected virtual void OnStart() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnFixedUpdate() { }
        protected virtual void OnLateUpdate() { }
        protected virtual void OnGameObjectEnable() { }
        protected virtual void OnGameObjectDisable() { }
        protected virtual void OnGameObjectDestroy() { }
        protected virtual void OnFocus(bool status) { }
        protected virtual void OnPause(bool status) { }

        private void Awake() => OnAwake();

        private void Start() => OnStart();

        private void Update() => OnUpdate();

        private void FixedUpdate() => OnFixedUpdate();

        private void LateUpdate() => OnLateUpdate();

        private void OnEnable() => OnGameObjectEnable();

        private void OnDisable() => OnGameObjectDisable();

        private void OnDestroy() => OnGameObjectDestroy();

        private void OnApplicationFocus(bool hasFocus) => OnFocus(hasFocus);

        private void OnApplicationPause(bool pauseStatus) => OnPause(pauseStatus);
    }
}