using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public interface INoticeEnemyArea
    {
        IReadOnlyAsyncReactiveProperty<bool> EnemyNoticed { get; }
        IReadOnlyAsyncReactiveProperty<bool> EnemyLeft { get; }
        public bool IsEnemyInsideArea { get; }
        public Transform Enemy { get; }

        void NoticeEnemy(Transform enemy);
        void EnemyLeave();
    }

    public class NoticeEnemyArea : INoticeEnemyArea
    {
        private readonly AsyncReactiveProperty<bool> _enemyNoticed = new(default);
        private readonly AsyncReactiveProperty<bool> _enemyLeft = new(default);

        public IReadOnlyAsyncReactiveProperty<bool> EnemyNoticed => _enemyNoticed;
        public IReadOnlyAsyncReactiveProperty<bool> EnemyLeft => _enemyLeft;
        public bool IsEnemyInsideArea { get; private set; }
        public Transform Enemy { get; private set; }

        public void NoticeEnemy(Transform enemy)
        {
            Enemy = enemy;
            IsEnemyInsideArea = true;
            _enemyNoticed.Value = true;
        }

        public void EnemyLeave()
        {
            IsEnemyInsideArea = false;
            _enemyLeft.Value = true;
        }
    }
}