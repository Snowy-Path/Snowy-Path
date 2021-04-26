using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class OptionSave
{
    public static string file =  "savesettings.json";
    public static string destination = Application.persistentDataPath + "/savesettings.json";
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
 
    public static void Save(OptionSettings option)
    {
        string json = JsonUtility.ToJson(option);
        File.WriteAllText(destination, json);

    }

    public static OptionSettings Load()
    {
        OptionSettings option = new OptionSettings();
        string json = ReadFromFile(file);
        option = JsonUtility.FromJson<OptionSettings>(json);
        return option;
    }



    public static void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStram = new FileStream(path, FileMode.Create);
        using(StreamWriter writer = new StreamWriter(fileStram)){
            writer.Write(json);
        }
    }

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
