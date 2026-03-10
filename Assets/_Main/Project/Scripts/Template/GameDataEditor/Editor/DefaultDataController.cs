using System.Linq;
using _Main.Project.Scripts.Template.GameDataEditor.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace _Main.Project.Scripts.Template.GameDataEditor.Editor
{
    public class DefaultDataController
    {
        private readonly string EditorDataHolderPath = "Assets/_Main/Project/Scripts/Template/GameDataEditor/Editor/DefaultPathEditor/EditorDataHolder.asset";
        public EditorDataHolder editorDataHolder;
        private VisualElement _root;
        private ListView listView;
        SerializedObject _serializedEditorData;
        private Foldout _mainFoldout;
        private VisualElement _addVisualElement;
        string _defaultDataInfo = "";
        string _dafultDataSaveFolderPath = "/_Main/Project/Scripts/Template/GameDataEditor/Data";
        private GameData _gamedData;

        public DefaultDataController(VisualElement root, GameData gameData)
        {
            _gamedData = gameData;
            _root = root;
            editorDataHolder = FileLoader.LoadFile<EditorDataHolder>(EditorDataHolderPath);
            for (var i = 0; i < editorDataHolder.DefaultDataPathes.Count; i++)
            {
                var path = editorDataHolder.DefaultDataPathes[i];
                int assetsIndex = path.IndexOf("_Main");
                if (assetsIndex != -1)
                {
                    var newPath = Application.dataPath + "/" + path.Substring(assetsIndex);
                    editorDataHolder.DefaultDataPathes[i] = newPath;
                }
            }

            Debug.Log($"EditorData holder Loaded.. {editorDataHolder.DefaultDataPathes.Count}");
            CreateList();
            _serializedEditorData = new SerializedObject(editorDataHolder);


        }
        private void CreateList()
        {
            _mainFoldout = new Foldout();
            _mainFoldout.text = "Default Json Files";

            listView = new ListView(editorDataHolder.DefaultDataPathes, 20, () =>
            {
                var fieldsContainer = new VisualElement();
                fieldsContainer.style.flexDirection = FlexDirection.Row;
                fieldsContainer.style.unityTextAlign = TextAnchor.MiddleCenter;
                var label = new Label();
                label.style.color = Color.red;
                // label.Bind(_serializedEditorData);

                var buttons = new VisualElement();
                buttons.style.flexDirection = FlexDirection.Row;

                var loadButton = new Button();
                loadButton.text = "Load";
                loadButton.style.width = 50;
                buttons.Add(loadButton);

                var selectFolderBtn = new Button();
                selectFolderBtn.text = "Save";
                selectFolderBtn.style.width = 35;
                selectFolderBtn.style.color = Color.yellow;
                buttons.Add(selectFolderBtn);

                var openFolderBtn = new Button();
                openFolderBtn.text = "Open";
                openFolderBtn.style.width = 30;
                openFolderBtn.style.color = Color.green;
                buttons.Add(openFolderBtn);

                var delete = new Button();
                delete.text = "Delete";
                delete.style.width = 40;
                delete.style.color = Color.red;

                buttons.Add(delete);




                fieldsContainer.Add(label);
                fieldsContainer.Add(buttons);

                return fieldsContainer;

            }, (element, index) =>
            {

                (element.ElementAt(0) as Label).text = GetSplitedPath(index);
                (element.ElementAt(1).ElementAt(0) as Button).clicked += () => LoadDefaultDataOnClick(index);
                (element.ElementAt(1).ElementAt(1) as Button).clicked += () => SaveFileDefault(index);
                (element.ElementAt(1).ElementAt(2) as Button).clicked += () => OpenFolder(index);
                (element.ElementAt(1).ElementAt(3) as Button).clicked += () => DeleteData(index);

                //   (element as Label).text = _editorDataHolder.DefaultDataPathes[index];
                //   (element as Button).clicked += () => LoadDefaultDataOnClick(index);
            });


            _addVisualElement = VisualElementFactori.CreateAddVisulaElement();
            //Debug.Log($"Count {_addVisualElement.childCount}");

            // (_addVisualElement.ElementAt(0) as TextField).value.Remove(0);
            (_addVisualElement.ElementAt(1) as Button).clicked += () => AddDefaultDataPath((_addVisualElement.ElementAt(0) as TextField).text);
            //(_addVisualElement.ElementAt(2) as Button).clicked += () => SelectDefaultDataPath();

            _mainFoldout.Add(_addVisualElement);
            _mainFoldout.Add(listView);
            _root.Add(_mainFoldout);

        }

        private string GetSplitedPath(int index)
        {
            return editorDataHolder.DefaultDataPathes[index].Split('/').Last();
        }

        private void DeleteData(int index)
        {
            ClearFoldout();
            CreateList();
            var cachedFile = editorDataHolder.DefaultDataPathes[index];
            editorDataHolder.DefaultDataPathes.RemoveAt(index);
            SaveManager.DeleteFile(cachedFile);
        }

        private void SelectDefaultDataPath()
        {

            // var selectedFilePath = EditorUtility.OpenFolderPanel("Select Folder", "Assets", "");
            // Debug.Log($"SelectDefaultDataFolder path={selectedFilePath}");
            // _dafultDataSaveFolderPath = selectedFilePath;
        }

        private void AddDefaultDataPath(string txt)
        {

            var fullPath = Application.dataPath + _dafultDataSaveFolderPath + "/" + txt + ".json";
            SaveManager.SaveDefaultData(_gamedData, fullPath);
            editorDataHolder.DefaultDataPathes.Add(fullPath);

            ClearFoldout();
            CreateList();
        }

        private void OpenFolder(int index)
        {
            var path = editorDataHolder.DefaultDataPathes[index];

            //EditorUtility.OpenFilePanel("Select Folder", path, "");
            EditorUtility.OpenWithDefaultApp(path);
        }

        private void LoadDefaultDataOnClick(int index)
        {
            Debug.Log($"LoadDefaultDataOnClick ={editorDataHolder.DefaultDataPathes[index]}");
            SaveManager.LoadDefaultData(_gamedData, editorDataHolder.DefaultDataPathes[index]);
            SaveManager.SaveData(_gamedData);
        }
        private void SaveFileDefault(int index)
        {
            SaveManager.SaveDefaultData(_gamedData, editorDataHolder.DefaultDataPathes[index]);

            // Debug.Log($"ListCount ={listView.ElementAt(0)}");

            // var selectedFilePath = EditorUtility.OpenFolderPanel("Select Folder", "", "");
            // if (selectedFilePath.Length < 10) return;
            // _editorDataHolder.DefaultDataPathes[index] = selectedFilePath;
            // //  CreateList();
            // Debug.Log($"Selected Folder:{selectedFilePath}  index {index} {listView.childCount}");


            ClearFoldout();
            CreateList();
        }

        private void ClearFoldout()
        {
            _root.Remove(_mainFoldout);
            //  _root.Remove(_addVisualElement);

        }
    }
}