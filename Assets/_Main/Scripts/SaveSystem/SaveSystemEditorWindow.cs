#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using SaveSystem;

namespace SaveSystem.Editor
{
    /// <summary>
    /// Unity Menu → Tools → Save System Editor
    /// 
    /// Özellikler:
    /// • Tüm save slot'larını listele
    /// • Slot'u Play'e basmadan yükle (editörde)
    /// • Ham JSON'u görüntüle / düzenle
    /// • Slot'u sil
    /// • Save klasörünü Finder/Explorer'da aç
    /// </summary>
    public class SaveSystemEditorWindow : EditorWindow
    {
        // ─── Pencere ─────────────────────────────────────────────────────────
        [MenuItem("Tools/Save System Editor")]
        public static void Open() => GetWindow<SaveSystemEditorWindow>("💾 Save System").minSize = new Vector2(600, 480);

        // ─── State ────────────────────────────────────────────────────────────
        private List<SlotEntry>  entries       = new();
        private int              selectedIndex = -1;
        private string           jsonPreview   = "";
        private bool             jsonEditable  = false;
        private Vector2          listScroll;
        private Vector2          jsonScroll;
        private string           saveDir       = "";
        private int              maxSlots      = 10;

        private static readonly Color ColorSlotFilled  = new Color(0.18f, 0.38f, 0.18f);
        private static readonly Color ColorSlotEmpty   = new Color(0.25f, 0.25f, 0.25f);
        private static readonly Color ColorSelected    = new Color(0.22f, 0.44f, 0.55f);

        // ─── Lifecycle ────────────────────────────────────────────────────────
        private void OnEnable()
        {
            RefreshSlots();
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        private void OnDisable() =>
            EditorApplication.playModeStateChanged -= OnPlayModeChanged;

        private void OnPlayModeChanged(PlayModeStateChange _) => RefreshSlots();

        // ─── GUI ─────────────────────────────────────────────────────────────
        private void OnGUI()
        {
            DrawToolbar();
            EditorGUILayout.BeginHorizontal();
            {
                DrawSlotList();
                DrawDetailPanel();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawToolbar()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                if (GUILayout.Button("↻ Yenile", EditorStyles.toolbarButton, GUILayout.Width(80)))
                    RefreshSlots();

                if (GUILayout.Button("📂 Klasör Aç", EditorStyles.toolbarButton, GUILayout.Width(100)))
                    EditorUtility.RevealInFinder(saveDir.Length > 0 ? saveDir : Application.persistentDataPath);

                GUILayout.FlexibleSpace();

                GUILayout.Label($"Save Dizini: {saveDir}", EditorStyles.miniLabel);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawSlotList()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(220));
            GUILayout.Label("SAVE SLOTLARI", EditorStyles.boldLabel);

            listScroll = EditorGUILayout.BeginScrollView(listScroll);
            {
                for (int i = 0; i < entries.Count; i++)
                {
                    var e       = entries[i];
                    var bgColor = i == selectedIndex ? ColorSelected : (e.Exists ? ColorSlotFilled : ColorSlotEmpty);

                    var prevBg = GUI.backgroundColor;
                    GUI.backgroundColor = bgColor;

                    if (GUILayout.Button(BuildSlotLabel(e), GUILayout.Height(56)))
                    {
                        selectedIndex = i;
                        LoadJsonPreview(e);
                    }
                    GUI.backgroundColor = prevBg;
                }
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.EndVertical();
        }

        private void DrawDetailPanel()
        {
            if (selectedIndex < 0 || selectedIndex >= entries.Count)
            {
                GUILayout.Label("← Soldan bir slot seç", EditorStyles.centeredGreyMiniLabel);
                return;
            }

            var entry = entries[selectedIndex];
            EditorGUILayout.BeginVertical();
            {
                // Başlık
                GUILayout.Label($"Slot {entry.SlotIndex} — {(entry.Exists ? entry.Info?.saveSlotName ?? "?" : "BOŞ")}", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

                if (entry.Exists)
                {
                    DrawSlotMetadata(entry);
                    GUILayout.Space(6);
                    DrawActionButtons(entry);
                    GUILayout.Space(6);
                    DrawJsonEditor(entry);
                }
                else
                {
                    EditorGUILayout.HelpBox("Bu slot boş.", MessageType.Info);
                }
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawSlotMetadata(SlotEntry e)
        {
            var info = e.Info;
            if (info == null) return;

            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Kaydedilme Tarihi", info.savedAt);
            EditorGUILayout.LabelField("Oynama Süresi",     info.FormattedPlayTime);
            EditorGUILayout.LabelField("Versiyon",          info.saveVersion);
            EditorGUILayout.LabelField("Dosya",             Path.GetFileName(info.filePath));
            EditorGUILayout.EndVertical();
        }

        private void DrawActionButtons(SlotEntry e)
        {
            EditorGUILayout.BeginHorizontal();
            {
                // Editor'da yükle (play moduna gerek yok)
                var prevColor = GUI.backgroundColor;
                GUI.backgroundColor = new Color(0.2f, 0.6f, 0.3f);
                if (GUILayout.Button("▶  Editörde Yükle", GUILayout.Height(28)))
                    LoadSlotInEditor(e);
                GUI.backgroundColor = prevColor;

                // Sil
                GUI.backgroundColor = new Color(0.7f, 0.2f, 0.2f);
                if (GUILayout.Button("🗑  Sil", GUILayout.Height(28), GUILayout.Width(70)))
                {
                    if (EditorUtility.DisplayDialog("Sil", $"Slot {e.SlotIndex} silinsin mi?", "Evet", "Hayır"))
                        DeleteSlot(e);
                }
                GUI.backgroundColor = prevColor;
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawJsonEditor(SlotEntry e)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Ham JSON", EditorStyles.boldLabel);
            jsonEditable = GUILayout.Toggle(jsonEditable, "Düzenle", EditorStyles.miniButton, GUILayout.Width(70));
            if (GUILayout.Button("💾 Kaydet JSON", EditorStyles.miniButton, GUILayout.Width(90)))
                SaveJsonDirectly(e);
            EditorGUILayout.EndHorizontal();

            jsonScroll = EditorGUILayout.BeginScrollView(jsonScroll, GUILayout.ExpandHeight(true));
            if (jsonEditable)
                jsonPreview = EditorGUILayout.TextArea(jsonPreview, GUILayout.ExpandHeight(true));
            else
                EditorGUILayout.TextArea(jsonPreview, EditorStyles.wordWrappedLabel, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();
        }

        // ─── İşlemler ────────────────────────────────────────────────────────

        private void LoadSlotInEditor(SlotEntry e)
        {
            // Sahnedeki ISaveable nesneleri bul ve restore et
            var saveables = FindObjectsOfType<MonoBehaviour>(true);
            int restored  = 0;

            foreach (var mb in saveables)
            {
                if (mb is ISaveable saveable)
                {
                    try
                    {
                        var data = JsonHelper.Deserialize<SaveData>(jsonPreview);
                        saveable.RestoreState(data);
                        EditorUtility.SetDirty(mb);
                        restored++;
                    }
                    catch { /* tip uyumsuzsa atla */ }
                }
            }

            // Eğer SaveManager varsa onu da bilgilendir
            if (SaveManager.Instance != null)
                Debug.Log($"[SaveSystemEditor] Slot {e.SlotIndex} editörde yüklendi. ({restored} ISaveable restore edildi)");
            else
                Debug.Log($"[SaveSystemEditor] Slot {e.SlotIndex} JSON yüklendi. (SaveManager sahnede bulunamadı, {restored} ISaveable restore edildi)");

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            ShowNotification(new GUIContent($"✓ Slot {e.SlotIndex} yüklendi!"));
        }

        private void DeleteSlot(SlotEntry e)
        {
            if (SaveManager.Instance != null)
            {
                SaveManager.Instance.DeleteSlot(e.SlotIndex);
            }
            else
            {
                if (File.Exists(e.FilePath)) File.Delete(e.FilePath);
            }
            selectedIndex = -1;
            jsonPreview   = "";
            RefreshSlots();
        }

        private void SaveJsonDirectly(SlotEntry e)
        {
            try
            {
                // Validate JSON
                JsonHelper.Deserialize<SaveData>(jsonPreview);
                File.WriteAllText(e.FilePath, jsonPreview);
                Debug.Log($"[SaveSystemEditor] Slot {e.SlotIndex} JSON olarak kaydedildi.");
                ShowNotification(new GUIContent("✓ JSON kaydedildi"));
                RefreshSlots();
            }
            catch (System.Exception ex)
            {
                EditorUtility.DisplayDialog("JSON Hatası", $"Geçersiz JSON:\n{ex.Message}", "Tamam");
            }
        }

        // ─── Yardımcı ─────────────────────────────────────────────────────────

        private void RefreshSlots()
        {
            entries.Clear();

            // Save dizinini bul
            saveDir = Application.isPlaying && SaveManager.Instance != null
                ? SaveManager.Instance.GetSaveDirectory()
                : Path.Combine(Application.persistentDataPath, "Saves");

            // maxSlots
            if (Application.isPlaying && SaveManager.Instance != null)
                maxSlots = SaveManager.Instance.MaxSlots;

            Directory.CreateDirectory(saveDir);

            for (int i = 0; i < maxSlots; i++)
            {
                var filePath = Path.Combine(saveDir, $"slot_{i:D2}.sav");
                var exists   = File.Exists(filePath);

                SaveSlotInfo info = null;
                if (exists)
                {
                    try
                    {
                        var json = File.ReadAllText(filePath);
                        var data = JsonHelper.Deserialize<SaveData>(json);
                        info = SaveSlotInfo.FromSaveData(data, filePath);
                    }
                    catch { /* bozuk dosya */ }
                }

                entries.Add(new SlotEntry { SlotIndex = i, FilePath = filePath, Exists = exists, Info = info });
            }

            // Seçili slot hâlâ geçerliyse preview'i yenile
            if (selectedIndex >= 0 && selectedIndex < entries.Count)
                LoadJsonPreview(entries[selectedIndex]);

            Repaint();
        }

        private void LoadJsonPreview(SlotEntry e)
        {
            if (!e.Exists) { jsonPreview = ""; return; }
            try   { jsonPreview = File.ReadAllText(e.FilePath); }
            catch { jsonPreview = "Dosya okunamadı."; }
        }

        private string BuildSlotLabel(SlotEntry e)
        {
            if (!e.Exists)
                return $"  Slot {e.SlotIndex}\n  <boş>";

            var info = e.Info;
            return info != null
                ? $"  Slot {e.SlotIndex}  ─  {info.saveSlotName}\n  ⏱ {info.FormattedPlayTime}   {info.savedAt[..10]}"
                : $"  Slot {e.SlotIndex}  ─  (okunamadı)";
        }

        // ─── Data Model ───────────────────────────────────────────────────────
        private class SlotEntry
        {
            public int          SlotIndex;
            public string       FilePath;
            public bool         Exists;
            public SaveSlotInfo Info;
        }
    }
}
#endif
