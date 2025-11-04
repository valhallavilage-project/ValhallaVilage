using CrossProject.Core.SaveLoad;

namespace CrossProject.Core
{
    public interface IMobsSpawnsService
    {
        void IncrementSpawnedMob(string spawnerId);
        int GetMobsSpawned(string id);
    }

    public class MobsSpawnsService : IMobsSpawnsService
    {
        private readonly GameStateManager _gameStateManager;

        public MobsSpawnsService(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }

        public void IncrementSpawnedMob(string spawnerId)
        {
            var part = _gameStateManager.State.Get<SpawnersPart>();
            part.IncrementSpawnedMob(spawnerId);
            _gameStateManager.Save();
        }

        public int GetMobsSpawned(string id)
        {
            var part = _gameStateManager.State.Get<SpawnersPart>();

            return part.GetMobsCount(id);
        }
    }
}
