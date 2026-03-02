using System;

namespace SaveSystem
{
    /// <summary>
    /// Tüm save data sınıflarının türemesi gereken base class.
    /// Kendi oyununa özel veriyi buradan türeterek ekle.
    /// </summary>
    [Serializable]
    public class SaveData
    {
        public string saveSlotName = "New Save";
        public string saveVersion   = "1.0";
        public string savedAt;          // ISO 8601 timestamp
        public float  totalPlayTime;    // saniye cinsinden
        public int    slotIndex;

        public SaveData()
        {
            savedAt = DateTime.UtcNow.ToString("o");
        }
    }
}
