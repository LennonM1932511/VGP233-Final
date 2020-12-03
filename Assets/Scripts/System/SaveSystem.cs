using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour, IGameModule
{
    private string saveFolderPath;
    public IEnumerator LoadModule()
    {
        Init();
        ServiceLocator.Register<SaveSystem>(this);
        yield return null;
    }

    public void SaveJSON<T>(T saveObject, string fileName)
    {
        if (!fileName.Contains(".txt"))
        {
            Debug.LogError("File should contain extension .txt");
            return;
        }

        string json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(saveFolderPath + "/" + fileName, json);
    }

    public T LoadJSON<T>(string fileName)
    {
        if (!fileName.Contains(".txt"))
        {
            Debug.LogError("File name incorrect");
            return default(T);
        }

        string savedString = File.ReadAllText(saveFolderPath + "/" + fileName);

        return JsonUtility.FromJson<T>(savedString);
    }

    // Save PlayerPrefs

    public void SavePlayerPrefs(float data, string key)
    {
        PlayerPrefs.SetFloat(key, data);
        PlayerPrefs.Save();
    }

    public void SavePlayerPrefs(int data, string key)
    {
        PlayerPrefs.SetInt(key, data);
        PlayerPrefs.Save();
    }

    public void SavePlayerPrefs(string data, string key)
    {
        PlayerPrefs.SetString(key, data);
        PlayerPrefs.Save();
    }

    // Load PlayerPrefs

    public int LoadInt(string key)
    {
        return PlayerPrefs.GetInt(key, 0);
    }

    public string LoadString(string key)
    {
        return PlayerPrefs.GetString(key, "");
    }

    // Clear PlayerPrefs

    public void ClearKey(string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
        }
    }

    public void ClearSavedPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    // Saving Binaries

    public void SaveBinary<T>(T data, string fileName)
    {
        FileStream dataStream = new FileStream(saveFolderPath + "/" + fileName + ".data", FileMode.Create);

        BinaryFormatter converter = new BinaryFormatter();
        converter.Serialize(dataStream, data);

        dataStream.Close();
    }

    // Loading Binaries

    public T LoadBinary<T>(string fileName)
    {
        string filePath = saveFolderPath + "/" + fileName + ".data";

        if (File.Exists(filePath))
        {
            FileStream dataStream = new FileStream(filePath, FileMode.Open);

            BinaryFormatter converter = new BinaryFormatter();
            T savedData = (T)converter.Deserialize(dataStream);

            dataStream.Close();
            
            return savedData;
        }
        else
        {
            Debug.LogError("Save File not found in " + filePath);
            return default;
        }
    }

    // SaveGame
    public void SaveGame()
    {
        // data toSave
        // assign values

        //ServiceLocator.Get<SaveSystem>
    }

    public void Init()
    {
        saveFolderPath = Application.dataPath + "/Saves/";

        if (!Directory.Exists(saveFolderPath))
        {
            Directory.CreateDirectory(saveFolderPath);
        }
    }
}
