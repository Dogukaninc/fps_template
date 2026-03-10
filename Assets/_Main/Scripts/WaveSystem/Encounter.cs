using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Scripts.WaveSystem
{
    [Serializable]
    public class Encounter
    {
        public EncounterSo encounterSo;
        // [ReadOnly] public List<EnemyController> enemies;
        [ReadOnly] public int currentEnemyCount;
        [ReadOnly] public int currentSpawnCount;
        [field: ReadOnly, SerializeField] public GameObject EnemyPrefab { get; private set; }
        
        public void Initialize()
        {
            if (encounterSo == null)
            {
                Debug.LogError("EncounterSo is not assigned.");
                return;
            }

            EnemyPrefab = encounterSo.EnemyPrefab;
            currentSpawnCount = encounterSo.SpawnCount;
            // enemies = new List<EnemyController>();
            // currentEnemyCount = enemies.Count;
        }

        // public void AddEnemy(GameObject enemyObject) => enemies.Add(enemyObject.GetComponent<EnemyController>());
        // public bool IsCompleted()
        // {
        //     return enemies.Count != 0 && enemies.All(e => e.IsDead);
        // }
    }
}