using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class OptionSave
{
    public static string file = "savesettings.json";
    public static string destination = Application.persistentDataPath + "/savesettings.json";
    /// <summary>
    /// Check if the file destination exist
    /// </summary>
    /// <returns></returns>
    public static bool Init()
    {

        if (File.Exists(destination))
        {
            return true;
        }

        else
        {
            return false;
        }
    }
    /// <summary>
    /// Create save of current option settings
    /// </summary>
    /// <param name="option"></param>
    public static void Save(OptionSettings option)
    {
        string json = JsonUtility.ToJson(option);
        File.WriteAllText(destination, json);

    }
    /// <summary>
    /// Load a current option settings save
    /// </summary>
    /// <returns></returns>
    public static OptionSettings Load()
    {
        OptionSettings option = new OptionSettings();
        string json = ReadFromFile(file);
        option = JsonUtility.FromJson<OptionSettings>(json);
        return option;
    }


    /// <summary>
    /// Return the json file
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static string ReadFromFile(string filename)
    {
        string path = GetFilePath(filename);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
            Debug.Log("File not found");
        return "";
    }

    public static string GetFilePath(string filename)
    {
        return Application.persistentDataPath + "/savesettings.json";
    }


}
