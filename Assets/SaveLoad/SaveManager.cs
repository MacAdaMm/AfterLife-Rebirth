using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public static void Save(string saveFile = "save")
    {
        Dictionary<string, object> state = LoadFile(saveFile);
        CaptureState(state);
        SaveFile(saveFile, state);
    }

    public static void Load(string saveFile = "save")
    {
        RestoreState(LoadFile(saveFile));
    }

    public static void Delete(string saveFile = "save")
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
        //print("Saving to " + path);
        using (FileStream stream = File.Open(path, FileMode.Create))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }

    private static void CaptureState(Dictionary<string, object> state)
    {
        foreach (SaveHandler saveHandler in GameObject.FindObjectsOfType<SaveHandler>(true))
        {
            state[saveHandler.GetUniqueIdentifier()] = saveHandler.CaptureState();
        }

        state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
    }

    private static void RestoreState(Dictionary<string, object> state)
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
