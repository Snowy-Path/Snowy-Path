using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public sealed class SaveSystem {
    private static SaveSystem instance;
    private SaveSystem() {
        // Input the nbr of saved possible
        saveDataArray = new SaveData[nbrSaveAllowed];
    }

    /// <summary>
    /// Singleton
    /// </summary>  
    public static SaveSystem Instance {
        get {
            if (instance == null) {
                instance = new SaveSystem();
            }
            return instance;
        }
    }

    // Saves
    UInt16 nbrSaveAllowed = 10;
    SaveData[] saveDataArray;

    /// <summary>
    /// Save the Data from a SaveData.
    /// The save is done in "save.dat" file.
    /// </summary>
    /// <param name="save">The save file</param>
    public static void SaveData(SaveData save) {
        string path = Application.persistentDataPath + "\\save_" + Array.IndexOf(SaveSystem.Instance.saveDataArray, save) + ".dat";
        Debug.Log("Serializing in : " + path);

        // Binary and Stream setup
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        try {
            // Formatter serialization data
            formatter.Serialize(stream, save);
        }
        catch (SerializationException e) {
            Debug.Log("Failed to serialize. Reason: " + e.Message);

        }
        finally {
            // Closing stream
            stream.Close();
        }
        Debug.Log("Saved done in location : " + path);

    }

    /// <summary>
    /// Load the data of a savefile
    /// </summary>
    /// <param name="path">Save file path</param>
    /// <returns></returns>
    public static SaveData LoadData(string path) {

        if (File.Exists(path)) {

            SaveData save = new SaveData();

            FileStream stream = new FileStream(path, FileMode.Open);
            try {
                // Binary and Stream setup
                BinaryFormatter formatter = new BinaryFormatter();

                // Return Datas
                save = formatter.Deserialize(stream) as SaveData;
            }
            catch (SerializationException e) {
                Debug.LogError("Failed to deserialize " + path + ". Reason: " + e.Message);
                save = null;
            }
            finally {
                // Closing stream
                stream.Close();
            }
            return save;
        }
        else {
            // Should never happen
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    /// <summary>
    /// Search for the existing save files and try to load them in the saveArray
    /// </summary>
    public void FindAndLoadExistingSave() {
        // Search for the dat file in the persistendDataPath Folder
        try {
            var datFiles = Directory.EnumerateFiles(Application.persistentDataPath, "*.dat");

            foreach (string currentFile in datFiles) {              
                string fileName = Path.GetFileNameWithoutExtension(currentFile);

                // We try to get the file slot number
                if (!UInt16.TryParse(fileName.Split('_')[1], out UInt16 fileNumber)){
                    // Error in the parse
                    Debug.LogWarning($"The file {fileName} cannot be loaded. Parse slot number failed");
                    continue;
                }
                if(fileNumber > nbrSaveAllowed) {
                    // Error the fileNumber is too high
                    Debug.LogWarning($"The file {fileName} cannot be loaded. Slot number too high");
                    continue;
                }

                // We load the data of the file in a SaveData object and put it at the right index of the saveDataArray
                SaveData saveLoaded = LoadData(currentFile);

                // Error while deserializing we skip this save
                if(saveLoaded == null) {
                    continue;
                }

                // Input the saveData in the saveDataArray
                saveDataArray[fileNumber] = saveLoaded;
                Debug.Log($"Save system : {fileName} loaded.");
            }
        }
        catch (Exception e) {
            Debug.LogError(e.Message);
        }
    }


    public void CreateNewSave(int slot) {
        // Create new SaveData
        SaveData newSave = new SaveData();
        newSave.headerData = new HeaderData();
        newSave.playerData = new PlayerData();
        saveDataArray[slot] = newSave;
        SaveData(newSave);

    }
}
