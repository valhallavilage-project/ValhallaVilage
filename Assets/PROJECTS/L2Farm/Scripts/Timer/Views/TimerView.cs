using System.Threading;
using CrossProject.Core.Camera;
using CrossProject.Core.Pooling;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace L2Farm
{
    public class TimerView : MonoBehaviour, IPoolElement
    {
        [SerializeField] private Transform vfxRoot;
        [SerializeField] private Transform uiRoot;
        [SerializeField] private TMP_Text timerLabel;
        [SerializeField] private Image progressBar;
        [SerializeField] private AudioSource audioSource;

        private CameraService _cameraService;
        private ITimersHandler _timersHandler;
        private string _timerId;
        private float _totalTime;

        private CancellationTokenSource _elapsedCts;
        
        public bool IsAvailableToGet { get; private set; }

        [Inject]
        private void AddDependencies(CameraService cameraService, ITimersHandler timersHandler)
        {
            _cameraService = cameraService;
            _timersHandler = timersHandler;
        }

        public void Setup(string timerId)
        {
            _timerId = timerId;
            _elapsedCts = new CancellationTokenSource();

            var data = _timersHandler.GetTimerData(timerId);

            _totalTime = data.Seconds;
            vfxRoot.GetChild(0).localScale = Vector3.one * data.VfxScale;

            _cameraService.AlignWithCamera(uiRoot);

            if (!data.SoundFx.IsEmpty)
            {
                audioSource.clip = data.SoundFx.Clip;
                audioSource.loop = data.SoundFx.IsLoop;
                audioSource.Play();
            }

            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(gameObject.GetCancellationTokenOnDestroy(), _elapsedCts.Token);

            _timersHandler.TimerElapsed.WithoutCurrent().Where(id => id.Equals(timerId)).ForEachAsync(Elapsed, linkedCts.Token).Forget();
        }

        private void Elapsed(string _)
        {
            progressBar.fillAmount = 1;
            timerLabel.text = "DONE";
            progressBar.color = Color.green;
            audioSource.Stop();
            
            _elapsedCts.Cancel();
            _elapsedCts.Dispose();
        }

        private void Update()
        {
            var seconds = _timersHandler.GetTimeLeft(_timerId);

            if (seconds < 0)
                return;

            progressBar.fillAmount = 1 - seconds / _totalTime;
            timerLabel.text = $"{seconds.ToTimeFormat()}";
        }

        public void SetPool(IPool pool)
        {
            
        }

        public void OnGet()
        {
            gameObject.SetActive(true);
            IsAvailableToGet = false;
        }

        public void OnReturn()
        {
            gameObject.SetActive(false);
            IsAvailableToGet = true;
        }
    }
}
