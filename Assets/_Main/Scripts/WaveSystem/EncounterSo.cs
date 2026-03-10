using UnityEngine;

namespace Scripts.WaveSystem
{
    [CreateAssetMenu(fileName = "so_encounter_config", menuName = "Scriptable Objects/so_encounter_config", order = 1)]
    public class EncounterSo : ScriptableObject
    {
        [field: SerializeField] public GameObject EnemyPrefab { get; private set; } // TODO-> Enemy'leri pool'dan alacak şekilde yeniden optimize edilecek (Prefab yerine pool tag ile değiştirilecek)
        [field: SerializeField] public int SpawnCount { get; private set; }
    }
}