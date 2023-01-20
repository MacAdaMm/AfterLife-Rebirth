using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShadyPixel.SaveLoad
{

    public static class SaveManager
    {
        /*
        public static IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                buildIndex = (int)state["lastSceneBuildIndex"];
            }
            yield return SceneManager.LoadSceneAsync(buildIndex);
            RestoreState(state);
        }
        */

        public static bool FileExists(string saveFile = "save")
        {
            string path = GetPathFromSaveFile(saveFile);
            return File.Exists(path);
        }

        public static void Save(string saveFile = "save")
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureSaveHandlerState(state);
            SaveFile(saveFile, state);
        }

        public static void Load(string saveFile = "save")
        {
            RestoreSaveHandlerState(LoadFile(saveFile));
        }

        public static void DeleteSaveFile(string saveFile = "save")
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        private static Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private static void SaveFile(string saveFile, object state)
        {
            string path = GetPathFromSaveFile(saveFile);
            //Debug.Log("Saving to " + path);
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private static void CaptureSaveHandlerState(Dictionary<string, object> state)
        {
            foreach (SaveHandler saveHandler in GameObject.FindObjectsOfType<SaveHandler>(true))
            {
                state[saveHandler.GetUniqueIdentifier()] = saveHandler.CaptureState();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private static void RestoreSaveHandlerState(Dictionary<string, object> state)
        {
            foreach (SaveHandler saveHandler in GameObject.FindObjectsOfType<SaveHandler>(true))
            {
                string id = saveHandler.GetUniqueIdentifier();
                if (state.ContainsKey(id))
                {
                    saveHandler.RestoreState(state[id]);
                }
            }
        }

        private static string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}