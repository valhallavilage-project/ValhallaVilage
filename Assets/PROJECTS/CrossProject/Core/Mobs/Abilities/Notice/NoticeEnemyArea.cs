using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public interface INoticeEnemyArea
    {
        IReadOnlyAsyncReactiveProperty<Transform> EnemyNoticed { get; }

        void NoticeEnemy(Transform enemy);
    }

    public class NoticeEnemyArea : INoticeEnemyArea
    {
        private readonly AsyncReactiveProperty<Transform> _enemyNoticed = new(default);

        public IReadOnlyAsyncReactiveProperty<Transform> EnemyNoticed => _enemyNoticed;

        public void NoticeEnemy(Transform enemy)
        {
            _enemyNoticed.Value = enemy;
        }
    }
}
