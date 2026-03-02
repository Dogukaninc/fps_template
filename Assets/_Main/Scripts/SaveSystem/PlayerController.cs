using UnityEngine;
using SaveSystem;
using SaveSystem.Example;

namespace SaveSystem.Example
{
    /// <summary>
    /// ISaveable implement eden örnek oyuncu scripti.
    /// </summary>
    public class PlayerController : MonoBehaviour, ISaveable
    {
        [SerializeField] private string playerName = "Hero";
        [SerializeField] private int    level      = 1;
        [SerializeField] private float  health     = 100f;

        // ISaveable
        public string SaveId => "PlayerController";

        public SaveData CaptureState()
        {
            return new GameSaveData
            {
                playerName = playerName,
                level      = level,
                health     = health,
                position   = transform.position,
                rotation   = transform.rotation,
            };
        }

        public void RestoreState(SaveData data)
        {
            if (data is not GameSaveData gsd) return;
            playerName          = gsd.playerName;
            level               = gsd.level;
            health              = gsd.health;
            transform.position  = gsd.position;
            transform.rotation  = gsd.rotation;
        }

        // ─── Örnek kullanım ──────────────────────────────────────────────────

        private void Update()
        {
            // F5 → Slot 0'a kaydet
            if (Input.GetKeyDown(KeyCode.F5))
                SaveManager.Instance.Save(0, (GameSaveData)CaptureState());

            // F9 → Slot 0'dan yükle
            if (Input.GetKeyDown(KeyCode.F9))
            {
                var d = SaveManager.Instance.Load<GameSaveData>(0);
                if (d != null) RestoreState(d);
            }
        }
    }
}
