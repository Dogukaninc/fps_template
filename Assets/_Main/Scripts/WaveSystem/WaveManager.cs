using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.Utilities;
using Scripts.WaveSystem;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class WaveManager : SingletonMonoBehaviour<WaveManager>
{
    /// <summary>
    /// General event olacak her combat scene yüklendiğinde raise edilecek.
    /// Raise edildiğinde Wave için geri sayım başlatılacak.
    /// Geri sayım bitince wave başlayacak.
    /// </summary>

    [field: SerializeField] public TextMeshProUGUI WaveCountDownText { get; private set; }

    [field: SerializeField] public float WaveCountDown { get; private set; } = 5f;
    [field: SerializeField] public List<Wave> Waves { get; private set; }
    public Wave CurrentWave { get; private set; }

    private void OnEnable()
    {
        // Events.OnCombatSceneLoaded += StartWave;
    }

    private void OnDisable()
    {
        // Events.OnCombatSceneLoaded -= StartWave;
    }

    private void Start()
    {
        CurrentWave = Waves[0];
        CurrentWave.Initialize();
    }

    [Button]
    private void StartWave()
    {
        if (Waves.Count <= 0) return;
        StartCoroutine(WaveCountDownStart());
    }

    private IEnumerator WaveCountDownStart()
    {
        while (WaveCountDown > 0)
        {
            WaveCountDown -= Time.deltaTime;
            WaveCountDownText.text = ((int)WaveCountDown).ToString();

            if (WaveCountDown <= 0)
            {
                WaveCountDown = 0;
                StartCoroutine(WaveSequence());
            }

            yield return null;
        }
    }

    private IEnumerator WaveSequence()
    {
        if (Waves.Count <= 0) yield break;
        CurrentWave.StartSpawning();

        // while (Waves.Any(w => !w.IsCompleted()))
        // {
        //     if (CurrentWave.CurrentEncounter.IsCompleted())
        //     {
        //         CurrentWave.LoadNextEncounter();
        //     }
        //
        //     yield return null;
        // }
    }

    public void LoadNextWave()
    {
        // if (CurrentWave.IsCompleted())
        // {
        //     int nextWaveIndex = Waves.IndexOf(CurrentWave) + 1;
        //     if (nextWaveIndex < Waves.Count)
        //     {
        //         CurrentWave = Waves[nextWaveIndex];
        //         CurrentWave.Initialize();
        //         CurrentWave.StartSpawning();
        //         Debug.Log("<color=green> Yeni wave yüklendi </color>");
        //     }
        //     else
        //     {
        //         Debug.Log("<color=blue> All Waves Completed !!! </color>");
        //     }
        // }
    }
}