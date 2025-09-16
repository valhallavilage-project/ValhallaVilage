using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CrossProject.Core
{
    public class MobCorpseView : MonoBehaviour
    {
        [SerializeField] private float _undergroundHeight;
        
        private float _decayTime;
        private float _startTime;

        private CancellationTokenSource _cts = new();
        
        public void StartDecay(float decayTime)
        {
            _decayTime = decayTime;
            _startTime = Time.unscaledTime;

            _cts.Cancel();
            _cts.Dispose();

            _cts = new CancellationTokenSource();

            Decay(_cts.Token).Forget();
        }

        private async UniTask Decay(CancellationToken cancellationToken)
        {
            var currentTransform = transform;
            
            while (Time.unscaledTime < _startTime + _decayTime)
            {
                var height = Time.deltaTime * _undergroundHeight / _decayTime;

                currentTransform.position -= new Vector3(0, height, 0);

                await UniTask.Yield(cancellationToken);
            }
            
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            _cts.Cancel();
            _cts.Dispose();
        }
    }
}
