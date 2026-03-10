using System.Collections;
using UnityEngine;

namespace Scripts.WaveSystem
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private float spawnInterval = 2f;
        [SerializeField] private CubeBounds cubeBounds;

        private float _defaultSpawnInterval;

        public Encounter Encounter;

        public void StartSpawner()
        {
            StartCoroutine(SpawningRoutine());
        }

        private void Spawn()
        {
            var enemy = Instantiate(Encounter.EnemyPrefab, cubeBounds.GetRandomGroundPoint(), Quaternion.identity);
            // Encounter.AddEnemy(enemy);
        }

        public IEnumerator SpawningRoutine()
        {
            while (Encounter.currentSpawnCount > 0)
            {
                if (Encounter.currentSpawnCount == 0) break;

                Spawn();
                yield return new WaitForSeconds(spawnInterval);
                Encounter.currentSpawnCount--;
            }
            
            Debug.Log("Encounter completed !!! , stopping spawn.");
        }
    }
}