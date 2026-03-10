using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.WaveSystem
{
    [Serializable]
    public class Wave
    {
        [field: SerializeField] public List<Encounter> Encounters { get; private set; }
        public Encounter CurrentEncounter { get; private set; }
        [field: SerializeField] public Spawner Spawner { get; private set; }

        public void Initialize()
        {
            if (Encounters == null || Encounters.Count == 0)
            {
                Debug.LogError("No encounters defined for this wave.");
                return;
            }

            CurrentEncounter = Encounters[0];
            CurrentEncounter.Initialize();
        }

        public void StartSpawning()
        {
            Spawner.Encounter = CurrentEncounter;
            Spawner.StartSpawner();
        }

        public void LoadNextEncounter()
        {
            int nextEncounterIndex = Encounters.IndexOf(CurrentEncounter) + 1;
            if (nextEncounterIndex < Encounters.Count)
            {
                CurrentEncounter = Encounters[nextEncounterIndex];
                CurrentEncounter.Initialize(); //TODO refactor
                StartSpawning();
            }
            else
            {
                Debug.Log("<color=yellow> No more encounters available in this wave. </color>");
                WaveManager.Instance.LoadNextWave();
            }
        }

        // public bool IsCompleted() => Encounters.TrueForAll(encounter => encounter.IsCompleted());
    }
}