using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem.Example
{
    /// <summary>
    /// Örnek oyuna özgü save verisi.
    /// Kendi oyunun için SaveData'dan türet ve istediğin alanları ekle.
    /// </summary>
    [Serializable]
    public class GameSaveData : SaveData
    {
        // ── Oyuncu ──
        public string    playerName  = "Hero";
        public int       level       = 1;
        public float     health      = 100f;
        public float     mana        = 50f;
        public Vector3   position;
        public Quaternion rotation;

        // ── Envanter ──
        public List<ItemData> inventory = new();

        // ── Dünya durumu ──
        public string    currentScene = "MainLevel";
        public int       gold         = 0;
        public int       questFlags   = 0;           // bit mask örneği
        public Dictionary<string, bool> discoveredAreas = new();
    }

    [Serializable]
    public class ItemData
    {
        public string itemId;
        public int    quantity;
        public float  durability;
    }
}
