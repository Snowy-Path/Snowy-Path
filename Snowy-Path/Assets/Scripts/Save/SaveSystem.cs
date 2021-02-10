using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveSystem
{
    
    /// <summary>
    /// Save the Data from multiple data source.
    /// The save is done in "save.dat" file.
    /// </summary>
    public static void SaveData() {

        // Binary and Stream setup
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save.dat";
        FileStream stream = new FileStream(path, FileMode.Create);

        // Formatter serialization data
        //PlayerData data = new PlayerData();
        //formatter.Serialize(stream, data);

        // Closing stream
        stream.Close();

        Debug.Log("Saved done in location : " + path);
        
    }
    public static void LoadData() {

        string path = Application.persistentDataPath + "/save.dat";

        if (File.Exists(path)) {

            // Binary and Stream setup
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            // Return Datas
            //PlayerData dataP = formatter.Deserialize(stream) as PlayerData;

            // Closing stream
            stream.Close();
        }
        else {

            Debug.LogError("Save file not found in " + path);
        }


    }
}
