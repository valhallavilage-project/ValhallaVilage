using System;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using VContainer;

namespace CrossProject.Core
{
    public class AttackAbilityView : MonoBehaviour
    {
        [TagField] [SerializeField] private string _expectedObjectTag;
        [SerializeField] private Collider _attackCollider;
        [SerializeField] private float _attackTimeOffset;

        private IDamageInfoProvider _damageInfoProvider;

        [Inject]
        private void AddDependencies(IAttackAbility attackAbility, IDamageInfoProvider damageInfoProvider)
        {
            _damageInfoProvider = damageInfoProvider;

            attackAbility.AttackBegin.WithoutCurrent().ForEachAwaitAsync(AttackBegan, gameObject.GetCancellationTokenOnDestroy()).Forget();
            attackAbility.AttackEnd.WithoutCurrent().ForEachAsync(AttackEnded, gameObject.GetCancellationTokenOnDestroy()).Forget();

            _attackCollider.GetAsyncTriggerEnterTrigger().ForEachAsync(TriggerEnter, gameObject.GetCancellationTokenOnDestroy()).Forget();

            _attackCollider.gameObject.SetActive(false);
            _attackCollider.isTrigger = true;
        }

        private async UniTask AttackBegan(bool _)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_attackTimeOffset));
            
            _attackCollider.gameObject.SetActive(true);
        }

        private void AttackEnded(bool _)
        {
            _attackCollider.gameObject.SetActive(false);
        }

        private void TriggerEnter(Collider enterObject)
        {
            if (!enterObject.gameObject.CompareTag(_expectedObjectTag))
            {
                return;
            }

            var damageReceivers = enterObject.gameObject.GetComponentsInChildren<IDamageReceiver>();

            if (damageReceivers.Length > 0)
            {
                damageReceivers[0].ReceiveDamage(_damageInfoProvider.Damage);
            }

            _attackCollider.gameObject.SetActive(false);
        }
    }
}
