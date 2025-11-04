using System.Collections.Generic;
using CrossProject.Core.SaveLoad;

namespace CrossProject.Core
{
    public class SpawnerData
    {
        public int MobsSpawned { get; set; }
    }
    
    public class SpawnersPart : IGameStatePart
    {
        public Dictionary<string, SpawnerData> SpawnersData { get; set; } = new();

        public void IncrementSpawnedMob(string spawnerId)
        {
            if (!SpawnersData.ContainsKey(spawnerId))
            {
                SpawnersData[spawnerId] = new SpawnerData();
            }

            SpawnersData[spawnerId].MobsSpawned++;
        }

        public int GetMobsCount(string spawnerId)
        {
            if (!SpawnersData.ContainsKey(spawnerId))
            {
                SpawnersData[spawnerId] = new SpawnerData();
            }

            return SpawnersData[spawnerId].MobsSpawned;
        }
    }
}
