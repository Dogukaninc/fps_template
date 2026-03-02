using System;

namespace SaveSystem
{
    /// <summary>
    /// Her save slot'u için hafif metadata.  
    /// UI'da save listesi gösterirken tüm save dosyasını yüklemeden bu nesneyi kullanabilirsin.
    /// </summary>
    [Serializable]
    public class SaveSlotInfo
    {
        public int    slotIndex;
        public string saveSlotName;
        public string savedAt;
        public float  totalPlayTime;
        public string saveVersion;
        public string filePath;

        // Okunabilir süre (HH:MM:SS)
        public string FormattedPlayTime
        {
            get
            {
                var ts = TimeSpan.FromSeconds(totalPlayTime);
                return $"{(int)ts.TotalHours:D2}:{ts.Minutes:D2}:{ts.Seconds:D2}";
            }
        }

        public static SaveSlotInfo FromSaveData(SaveData data, string filePath)
        {
            return new SaveSlotInfo
            {
                slotIndex     = data.slotIndex,
                saveSlotName  = data.saveSlotName,
                savedAt       = data.savedAt,
                totalPlayTime = data.totalPlayTime,
                saveVersion   = data.saveVersion,
                filePath      = filePath
            };
        }
    }
}
