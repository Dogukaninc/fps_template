using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// Ana Save/Load yöneticisi. Sahneye bir kez ekle (Singleton).
    /// </summary>
    public class SaveManager : MonoBehaviour
    {
        // ─── Singleton ────────────────────────────────────────────────────────
        public static SaveManager Instance { get; private set; }

        // ─── Inspector Ayarları ───────────────────────────────────────────────
        [Header("Ayarlar")]
        [Tooltip("Kaç save slot desteklensin?")]
        [SerializeField] private int maxSlots = 5;

        [Tooltip("Save dosyaları şifreli kaydedilsin mi?")]
        [SerializeField] private bool encryptSaves = false;

        [Tooltip("Şifreleme anahtarı (encryptSaves açıksa)")]
        [SerializeField] private string encryptionKey = "MY_SECRET_KEY_32";

        // ─── Olaylar ──────────────────────────────────────────────────────────
        public event Action<int>      OnSaveCompleted;   // slot index
        public event Action<int>      OnLoadCompleted;   // slot index
        public event Action<int>      OnSlotDeleted;     // slot index
        public event Action<string>   OnError;

        // ─── Iç Durum ─────────────────────────────────────────────────────────
        private string SaveDirectory => Path.Combine(Application.persistentDataPath, "Saves");
        private const string FILE_EXTENSION = ".sav";
        private const string META_FILE      = "slots_meta.json";

        private List<SaveSlotInfo>  cachedMeta  = new();
        private float               sessionStartTime;

        // ─── Unity Lifecycle ─────────────────────────────────────────────────
        private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Directory.CreateDirectory(SaveDirectory);
            RefreshMeta();
            sessionStartTime = Time.realtimeSinceStartup;
        }

        // ─── Public API ───────────────────────────────────────────────────────

        /// <summary>Belirtilen slot'a kayıt yapar.</summary>
        public bool Save<T>(int slotIndex, T data) where T : SaveData
        {
            if (!ValidateSlot(slotIndex)) return false;
            try
            {
                // Toplam oynama süresi
                data.slotIndex     = slotIndex;
                data.savedAt       = DateTime.UtcNow.ToString("o");
                data.totalPlayTime = GetAccumulatedPlayTime(slotIndex) + (Time.realtimeSinceStartup - sessionStartTime);
                data.saveVersion   = Application.version;

                var json = JsonHelper.Serialize(data);
                var raw  = encryptSaves ? Encrypt(json) : json;
                File.WriteAllText(GetFilePath(slotIndex), raw);

                UpdateMeta(slotIndex, data);
                OnSaveCompleted?.Invoke(slotIndex);
                Debug.Log($"[SaveManager] Slot {slotIndex} kaydedildi → {GetFilePath(slotIndex)}");
                return true;
            }
            catch (Exception ex)
            {
                var msg = $"[SaveManager] Kaydetme hatası (Slot {slotIndex}): {ex.Message}";
                Debug.LogError(msg);
                OnError?.Invoke(msg);
                return false;
            }
        }

        /// <summary>Belirtilen slot'u yükler. Başarısızsa null döner.</summary>
        public T Load<T>(int slotIndex) where T : SaveData
        {
            if (!ValidateSlot(slotIndex)) return null;
            var path = GetFilePath(slotIndex);
            if (!File.Exists(path))
            {
                Debug.LogWarning($"[SaveManager] Slot {slotIndex} bulunamadı: {path}");
                return null;
            }
            try
            {
                var raw  = File.ReadAllText(path);
                var json = encryptSaves ? Decrypt(raw) : raw;
                var data = JsonHelper.Deserialize<T>(json);
                sessionStartTime = Time.realtimeSinceStartup;   // yeni oturum başlıyor
                OnLoadCompleted?.Invoke(slotIndex);
                Debug.Log($"[SaveManager] Slot {slotIndex} yüklendi.");
                return data;
            }
            catch (Exception ex)
            {
                var msg = $"[SaveManager] Yükleme hatası (Slot {slotIndex}): {ex.Message}";
                Debug.LogError(msg);
                OnError?.Invoke(msg);
                return null;
            }
        }

        /// <summary>Slot'u siler.</summary>
        public bool DeleteSlot(int slotIndex)
        {
            if (!ValidateSlot(slotIndex)) return false;
            var path = GetFilePath(slotIndex);
            if (File.Exists(path)) File.Delete(path);
            cachedMeta.RemoveAll(m => m.slotIndex == slotIndex);
            SaveMeta();
            OnSlotDeleted?.Invoke(slotIndex);
            Debug.Log($"[SaveManager] Slot {slotIndex} silindi.");
            return true;
        }

        /// <summary>Tüm dolu slot'ların metadata listesini döner (UI için kullan).</summary>
        public List<SaveSlotInfo> GetAllSlotInfos() => new(cachedMeta);

        /// <summary>Belirli slot'un bilgisini döner. Slot boşsa null.</summary>
        public SaveSlotInfo GetSlotInfo(int slotIndex) =>
            cachedMeta.FirstOrDefault(m => m.slotIndex == slotIndex);

        /// <summary>Slot dolu mu?</summary>
        public bool SlotExists(int slotIndex) => File.Exists(GetFilePath(slotIndex));

        public int MaxSlots => maxSlots;

        // ─── ISaveable Toplu Kayıt (isteğe bağlı) ────────────────────────────

        /// <summary>
        /// Sahnedeki tüm ISaveable nesnelerinden veri toplayıp bir Dictionary'ye koyar.
        /// İstersen bunu SaveData türevine göm.
        /// </summary>
        public Dictionary<string, string> CaptureAllSaveables()
        {
            var map = new Dictionary<string, string>();
            foreach (var saveable in FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>())
            {
                var data = saveable.CaptureState();
                map[saveable.SaveId] = JsonHelper.Serialize(data);
            }
            return map;
        }

        /// <summary>ISaveable nesnelere veriyi geri dağıtır.</summary>
        public void RestoreAllSaveables(Dictionary<string, string> map)
        {
            foreach (var saveable in FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveable>())
            {
                if (map.TryGetValue(saveable.SaveId, out var json))
                {
                    var data = JsonHelper.Deserialize<SaveData>(json);
                    saveable.RestoreState(data);
                }
            }
        }

        // ─── Editor / Debug Yardımcıları ─────────────────────────────────────

        /// <summary>Ham JSON'u döner (Editor tool için).</summary>
        public string GetRawJson(int slotIndex)
        {
            var path = GetFilePath(slotIndex);
            if (!File.Exists(path)) return null;
            var raw = File.ReadAllText(path);
            return encryptSaves ? Decrypt(raw) : raw;
        }

        /// <summary>Save dizini yolunu döner (Editor tool için).</summary>
        public string GetSaveDirectory() => SaveDirectory;

        public string GetFilePath(int slotIndex) =>
            Path.Combine(SaveDirectory, $"slot_{slotIndex:D2}{FILE_EXTENSION}");

        // ─── Yardımcı Metodlar ────────────────────────────────────────────────

        private bool ValidateSlot(int slotIndex)
        {
            if (slotIndex >= 0 && slotIndex < maxSlots) return true;
            Debug.LogError($"[SaveManager] Geçersiz slot index: {slotIndex}. Maksimum: {maxSlots - 1}");
            return false;
        }

        private float GetAccumulatedPlayTime(int slotIndex)
        {
            var info = GetSlotInfo(slotIndex);
            return info?.totalPlayTime ?? 0f;
        }

        private void UpdateMeta(int slotIndex, SaveData data)
        {
            cachedMeta.RemoveAll(m => m.slotIndex == slotIndex);
            cachedMeta.Add(SaveSlotInfo.FromSaveData(data, GetFilePath(slotIndex)));
            cachedMeta = cachedMeta.OrderBy(m => m.slotIndex).ToList();
            SaveMeta();
        }

        private void SaveMeta()
        {
            var json = JsonHelper.Serialize(cachedMeta);
            File.WriteAllText(Path.Combine(SaveDirectory, META_FILE), json);
        }

        private void RefreshMeta()
        {
            var metaPath = Path.Combine(SaveDirectory, META_FILE);
            if (!File.Exists(metaPath)) { cachedMeta = new(); return; }
            try   { cachedMeta = JsonHelper.Deserialize<List<SaveSlotInfo>>(File.ReadAllText(metaPath)) ?? new(); }
            catch { cachedMeta = new(); }
        }

        // ─── Basit XOR Şifreleme ──────────────────────────────────────────────

        private string Encrypt(string plain)
        {
            var key    = encryptionKey;
            var chars  = plain.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
                chars[i] = (char)(chars[i] ^ key[i % key.Length]);
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(new string(chars)));
        }

        private string Decrypt(string cipher)
        {
            var bytes  = Convert.FromBase64String(cipher);
            var plain  = System.Text.Encoding.UTF8.GetString(bytes).ToCharArray();
            var key    = encryptionKey;
            for (int i = 0; i < plain.Length; i++)
                plain[i] = (char)(plain[i] ^ key[i % key.Length]);
            return new string(plain);
        }
    }
}
