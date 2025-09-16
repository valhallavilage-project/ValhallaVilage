using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

namespace CrossProject.Core
{
    public interface IAgroArea
    {
        public bool IsEnemyInsideArea { get; }
        public Transform Enemy { get; }
        
        void Init(Collider agroZone);
        void ForgotEnemy();
    }

    public class AgroArea : IAgroArea, IDisposable
    {
        private CancellationTokenSource _disposeCts = new();
        private CancellationTokenSource _agroZoneCts = new();

        public bool IsEnemyInsideArea { get; private set; }
        public Transform Enemy { get; private set; }

        public AgroArea(INoticeEnemyArea noticeEnemyArea, IDieAbility dieAbility)
        {
            noticeEnemyArea.EnemyNoticed.WithoutCurrent().ForEachAsync(NoticeEnemy, _disposeCts.Token).Forget();
            dieAbility.DeathCompleted.WithoutCurrent().ForEachAsync(Die, _disposeCts.Token).Forget();
        }

        public void Init(Collider agroZone)
        {
            var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_agroZoneCts.Token, _disposeCts.Token);
            
            agroZone.GetAsyncTriggerExitTrigger().ForEachAsync(EnemyLeave, linkedCts.Token).Forget();
        }

        private void NoticeEnemy(Transform enemy)
        {
            IsEnemyInsideArea = true;
            Enemy = enemy;
        }

        private void EnemyLeave(Collider exitObject)
        {
            if (!IsEnemyInsideArea || !exitObject.gameObject.CompareTag(Enemy.tag))
            {
                return;
            }
            
            IsEnemyInsideArea = false;
        }

        public void ForgotEnemy()
        {
            Enemy = null;
        }

        private void Die(bool _)
        {
            _agroZoneCts.Cancel();
            _agroZoneCts.Dispose();
            _agroZoneCts = new CancellationTokenSource();
            IsEnemyInsideArea = false;
            Enemy = null;
        }

        public void Dispose()
        {
            Enemy = null;
            _disposeCts?.Cancel();
            _disposeCts?.Dispose();
        }
    }
}
