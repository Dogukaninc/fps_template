namespace SaveSystem
{
    /// <summary>
    /// Save sistemine kayıt olabilmek için MonoBehaviour'larının implement etmesi gereken arayüz.
    /// </summary>
    public interface ISaveable
    {
        /// <summary>Sisteme benzersiz bir kimlik döner (örn. "PlayerController", "InventoryManager").</summary>
        string SaveId { get; }

        /// <summary>Mevcut durumu bir SaveData nesnesine dönüştürür.</summary>
        SaveData CaptureState();

        /// <summary>SaveData nesnesinden durumu geri yükler.</summary>
        void RestoreState(SaveData data);
    }
}
