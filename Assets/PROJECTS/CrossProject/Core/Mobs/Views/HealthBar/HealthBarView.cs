using CrossProject.Core.Camera;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace CrossProject.Core
{
    public class HealthBarView : MonoBehaviour
    {
        [SerializeField] private Image _healthBarFill;

        private IHealthHandler _healthHandler;
        private CameraService _cameraService;

        [Inject]
        private void AddDependencies(IHealthHandler healthHandler, CameraService cameraService)
        {
            _healthHandler = healthHandler;
            _cameraService = cameraService;

            healthHandler.Health.WithoutCurrent().ForEachAsync(HealthChanged, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void HealthChanged(float current)
        {
            _healthBarFill.fillAmount = current / _healthHandler.MaxHealth.Value;
        }

        public void LateUpdate()
        {
            if (_cameraService.MainCamera == null)
            {
                return;
            }

            transform.rotation = Quaternion.LookRotation(-_cameraService.MainCamera.transform.forward, _cameraService.MainCamera.transform.up);
        }
    }
}
