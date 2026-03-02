#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using SaveSystem;

namespace SaveSystem.Editor
{
    /// <summary>SaveManager için gelişmiş Inspector görünümü.</summary>
    [CustomEditor(typeof(SaveManager))]
    public class SaveManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var mgr = (SaveManager)target;
            GUILayout.Space(8);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("💾 Save System Editor Aç"))
                SaveSystemEditorWindow.Open();

            if (GUILayout.Button("📂 Klasör Aç"))
                EditorUtility.RevealInFinder(Application.persistentDataPath);
            EditorGUILayout.EndHorizontal();

            if (Application.isPlaying)
            {
                GUILayout.Space(4);
                GUILayout.Label("Mevcut Slotlar", EditorStyles.boldLabel);
                foreach (var info in mgr.GetAllSlotInfos())
                {
                    EditorGUILayout.LabelField(
                        $"Slot {info.slotIndex}: {info.saveSlotName}",
                        $"⏱ {info.FormattedPlayTime}");
                }
            }
        }
    }
}
#endif
