using Cinemachine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class NoticeEnemyAbilityView : MonoBehaviour
    {
        [TagField] [SerializeField] private string _expectedObjectTag;
        [SerializeField] private Collider _areaCollider;
        [SerializeField] private Transform _eyesPosition;

        private INoticeEnemyArea _area;

        [Inject]
        public void AddDependencies(INoticeEnemyArea area)
        {
            _area = area;
        }

        protected void Start()
        {
            _areaCollider.GetAsyncTriggerEnterTrigger().ForEachAsync(OnTriggerEnter, gameObject.GetCancellationTokenOnDestroy()).Forget();
        }

        private void OnTriggerEnter(Collider enterObject)
        {
            if (!enterObject.gameObject.CompareTag(_expectedObjectTag))
            {
                return;
            }

            if (_eyesPosition.position != Vector3.zero)
            {
                var distance = enterObject.bounds.center - _eyesPosition.position;

                var isHit = Physics.Raycast(
                    _eyesPosition.position,
                    distance.normalized,
                    out _,
                    distance.magnitude);

                if (isHit)
                {
                    _area.NoticeEnemy(enterObject.transform);
                }
            }
            else
            {
                _area.NoticeEnemy(enterObject.transform);
            }
        }
    }
}
