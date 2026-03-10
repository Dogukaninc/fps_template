using System.IO;
using UnityEngine;

namespace _Main.Project.Scripts.Template.GameDataEditor.Scripts
{
    public static class SaveManager
    {
        public static GameData gameData;
        private static string path = "/Data/";
        private static string defaultPath = "/DefaultData/";
        public static void Initialize(GameData gameDataIn)
        {
            gameData = gameDataIn;
            LoadData(gameData);


        }
        public static void SaveData(ScriptableObject so) => SaveDataAtPath(so, path);

        public static void SaveDefaultData(ScriptableObject so) => SaveDefaultDataAtPath(so, defaultPath);
        public static void SaveDefaultData(ScriptableObject so, string fullPath) => SaveDefaultDataAtFullPath(so, fullPath);
        //public static string DefaultPath => Application.persistentDataPath;
        public static string DefaultPath => "Assets/_Main/Project/Scripts/Template/GameDataEditor/Data/";

        private static void SaveDataAtPath(ScriptableObject so, string pathIn)
        {

#if UNITY_EDITOR

            string filePath = Application.dataPath + "/_Main/Project/Scripts/Template/GameDataEditor/Data/" + "GameData.json";
            CreateFolder(Application.dataPath + "/_Main/Project/Scripts/Template/GameDataEditor/Data");
#endif

#if !UNITY_EDITOR
        
        string filePath = Application.persistentDataPath + pathIn + "GameData.json";
        CreateFolder(Application.persistentDataPath + pathIn);
#endif

            var soToJson = JsonUtility.ToJson(so);
            File.WriteAllText(filePath, soToJson);
            //  Debug.Log($"Saved Data..at:{filePath}");
        }
        private static void SaveDefaultDataAtPath(ScriptableObject so, string pathIn)
        {
            string filePath = DefaultPath + pathIn + "GameData.json";
            CreateFolder(DefaultPath + pathIn);

            var soToJson = JsonUtility.ToJson(so);
            File.WriteAllText(filePath, soToJson);
            //  Debug.Log($"Saved Data..at:{filePath}");
        }
        private static void SaveDefaultDataAtFullPath(ScriptableObject so, string pathIn)
        {

            Debug.LogWarning($"SaveDefaultDataAtFullPath pathIn={pathIn}");
            // CreateFolder(pathIn);

            var soToJson = JsonUtility.ToJson(so);
            File.WriteAllText(pathIn, soToJson);
            Debug.LogWarning($"Saved Data..at:{pathIn}");
            //  Debug.Log($"Saved Data..at:{filePath}");
        }

        public static T LoadData<T>(T data) where T : UnityEngine.Object
        {



            // string folderPath = Application.persistentDataPath + path;
            // string filePath = folderPath + "GameData.json";

#if UNITY_EDITOR
            string folderPath = Application.dataPath + "/_Main/Project/Scripts/Template/GameDataEditor/Data/";
            string filePath = folderPath + "GameData.json";
            //   CreateFolder(Application.dataPath + "/Main/_Project/Scripts/Template/GameDataEditor/Data");
#endif

#if !UNITY_EDITOR
        string folderPath=Application.persistentDataPath+path;
         string filePath = folderPath+ "GameData.json";
#endif

            Debug.Log(folderPath);
            Debug.Log(filePath);

            // if (!Directory.Exists(folderPath)) SaveData(data as ScriptableObject);
            if (!File.Exists(filePath)) SaveData(data as ScriptableObject);


            var txt = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(txt, data);
            Debug.Log("Loaded Data..");

            return data;
        }

        public static T LoadDefaultData<T>(T data, string fullPath) where T : UnityEngine.Object
        {
            if (!File.Exists(fullPath)) SaveDefaultData(data as ScriptableObject, fullPath);
            var txt = File.ReadAllText(fullPath);
            JsonUtility.FromJsonOverwrite(txt, data);
            Debug.Log("Loaded Data..");

            return data;
        }

        static void CreateFolder(string pathIn)
        {
            if (Directory.Exists(pathIn) == false)
            {
                Directory.CreateDirectory(pathIn);
                Debug.Log("Created Path" + pathIn);
            }

            // ShowExplorer(pathIn);
        }


        public static void DeleteFile(string v)
        {
            // var newDirection = v.Replace(@"/", @"\");
            //  newDirection = newDirection.Remove(newDirection.Length - 4);

            File.Delete(v);

            // v = v.Replace(@"/", @"\");
            // string[] splitedPath = v.Split(@"\");
            // string newPath = "";
            // bool canAdd = false;
            // foreach (var item in splitedPath)
            // {
            //     Debug.Log("----" + item);

            //     if (item == "Assets") canAdd = true;
            //     if (canAdd)
            //     {

            //         newPath += item + @"\";
            //     }

            // }
            // newPath = newPath.Remove(newPath.Length - 1);
            // Debug.LogError(newPath);
            // Directory.Delete(newPath, true);
        }
    }
}