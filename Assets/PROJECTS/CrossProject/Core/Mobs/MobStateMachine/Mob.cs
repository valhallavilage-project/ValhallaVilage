using CrossProject.Core.Interactions;
using CrossProject.Core.Pooling;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CrossProject.Core
{
    public class Mob : AbstractInteractiveObject, IPoolElement
    {
        private MobsSpawnPoint _spawnPoint;

        public bool IsAvailableToGet { get; private set; }


        public void SetPool(IPool pool)
        {
        }

        public void OnGet()
        {
            IsAvailableToGet = false;
        }

        public void OnReturn()
        {
            _spawnPoint.RemoveMob();
            IsAvailableToGet = true;
        }

        public void BindSpawnPoint(MobsSpawnPoint spawnPoint)
        {
            _spawnPoint = spawnPoint;
        }

        protected override UniTask AfterInteraction()
        {
            return UniTask.CompletedTask;
        }
    }
}
