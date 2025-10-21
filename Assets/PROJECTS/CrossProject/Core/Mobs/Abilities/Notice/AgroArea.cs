using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using VContainer.Unity;

namespace CrossProject.Core
{
    public interface IAgroArea
    {
        public bool IsEnemyInsideArea { get; }
        public Transform Enemy { get; }
        bool IsEnemyInactive { get; }

        void Init(Collider agroZone);
        void ForgetEnemy();
    }

    public class AgroArea : IAgroArea, IDisposable, ITickable
    {
        private CancellationTokenSource _disposeCts = new();
        private CancellationTokenSource _agroZoneCts = new();
        private Collider _agroZone;

        public bool IsEnemyInsideArea { get; private set; }
        public bool IsEnemyInactive { get; private set; }
        public Transform Enemy { get; private set; }

        public AgroArea(INoticeEnemyArea noticeEnemyArea, IDieAbility dieAbility)
        {
            noticeEnemyArea.EnemyNoticed.WithoutCurrent().ForEachAsync(NoticeEnemy, _disposeCts.Token).Forget();
            dieAbility.DeathCompleted.Listen(Die, _disposeCts.Token);
        }

        public void Init(Collider agroZone)
        {
            _agroZone = agroZone;
        }

        private void NoticeEnemy(Transform enemy)
        {
            IsEnemyInsideArea = true;
            Enemy = enemy;
        }

        public void ForgetEnemy()
        {
            Enemy = null;
        }

        public void Tick()
        {
            IsEnemyInactive = Enemy == null || !Enemy.gameObject.activeInHierarchy;
            
            if (!IsEnemyInsideArea)
            {
                return;
            }
            
            IsEnemyInsideArea = Enemy.gameObject.activeInHierarchy && IsInside(_agroZone, Enemy);
        }

        private bool IsInside(Collider zone, Transform target)
        {
            var closest = zone.ClosestPoint(target.position);

            return closest == target.position;
        }

        private void Die()
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
