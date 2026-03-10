using _Main.Project.Scripts.Template.GameDataEditor.Scripts;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

/*
-----To Do list------
*Clear all player prefs 
*/
namespace _Main.Project.Scripts.Template.GameDataEditor.Editor
{
    
    public class GameDataEditor : EditorWindow
    {
        [MenuItem("Window/UI Toolkit/GameDataEditor")]
        public static void ShowExample()
        {
            GameDataEditor wnd = GetWindow<GameDataEditor>();
            wnd.titleContent = new GUIContent("GameDataEditor");
        }

        public string dataPath;
        public GameData gameData;
        private DefaultDataController _defaultDataController;

        public void CreateGUI()
        {
            LoadUxmlAndUss();
            LoadDatas();
            CreateDataElements();
            ButtonBinding();
            _defaultDataController = new(rootVisualElement, gameData);
        }

        private void ButtonBinding()
        {
            var root = rootVisualElement;
            var saveData = rootVisualElement.Q<Button>("SaveData");
           var loadData = rootVisualElement.Q<Button>("LoadData");
            // var openData = rootVisualElement.Q<Button>("OpenData");
            // var saveAsDeafultData = rootVisualElement.Q<Button>("SaveAsDeafultData");
            // var loadDefaultData = rootVisualElement.Q<Button>("LoadDefaultData");
            saveData.clicked += SaveData;
//             loadData.clicked += LoadData;
            // openData.clicked += OpenFolder;

            // saveAsDeafultData.clicked += SaveAsDeafultData;
            // loadDefaultData.clicked += LoadDefaultData;
        }

        private void LoadData()
        {
            gameData = SaveManager.LoadData(gameData);
        }

        private void SaveData()
        {
            SaveManager.SaveData(gameData);
        }

        private void CreateDataElements()
        {
            var fieldsContainer = new ScrollView(); // ScrollView oluşturuluyor
            fieldsContainer.style.flexGrow = 1; // Görünümün tamamını kaplaması için stil ayarı
            rootVisualElement.Add(fieldsContainer); // ScrollView kök eleman olarak ekleniyor
            
            SerializedObject serializedObject = new SerializedObject(gameData);
            SerializedProperty property = serializedObject.GetIterator();

            // Move to the first property
            bool hasNext = property.NextVisible(true);
            while (hasNext)
            {
                if (property.name == "m_Script")
                {
                    hasNext = property.NextVisible(false);
                    continue;
                }

                var field = new PropertyField(property.Copy());
                field.Bind(serializedObject);
                fieldsContainer.Add(field);

                hasNext = property.NextVisible(false);
            }
        }

        private void LoadDatas()
        {
            dataPath = "Assets/_Main/Project/Scripts/Template/GameDataEditor/Scripts/GameData.asset";
            gameData = FileLoader.LoadFile<GameData>(dataPath);
        }

        private void LoadUxmlAndUss()
        {
            // Each editor window contains a root VisualElement object
            VisualElement root = rootVisualElement;

            // VisualElements objects can contain other VisualElement following a tree hierarchy.
            // VisualElement label = new Label("Hello World! From C#");
            // root.Add(label);

            // Import UXML
            var visualTree =
                AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/_Main/Project/Scripts/Template/GameDataEditor/Editor/GameDataEditor.uxml");
            VisualElement labelFromUXML = visualTree.Instantiate();
            root.Add(labelFromUXML);

            // A stylesheet can be added to a VisualElement.
            // The style will be applied to the VisualElement and all of its children.
            // var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/7-GameDataEditor/Editor/GameDataEditor.uss");
            // VisualElement labelWithStyle = new Label("Hello World! With Style");
            // labelWithStyle.styleSheets.Add(styleSheet);
            // root.Add(labelWithStyle);
        }
    }

    public static class FileLoader
    {
        public static T LoadFile<T>(string path) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    }
}