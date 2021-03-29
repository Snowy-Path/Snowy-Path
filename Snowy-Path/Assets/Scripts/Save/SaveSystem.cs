using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public sealed class SaveSystem : MonoBehaviour {

    public static SaveSystem Instance;

    // Saves
    public int currentSave;
    UInt16 nbrSaveAllowed = 10;
    SaveData[] saveDataArray;

    private void Awake() {

        // Singleton of the GameManager
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(Instance);
            Instance = gameObject.GetComponent<SaveSystem>();
            return;
        }

        DontDestroyOnLoad(this.gameObject);

        // Input the nbr of saved possible
        saveDataArray = new SaveData[nbrSaveAllowed];
        FindAndLoadExistingSave();

    }

    /// <summary>
    /// Save the Data from a SaveData.
    /// The save is done in "save.dat" file.
    /// </summary>
    /// <param name="save">The save file</param>
    private static void SaveFile(SaveData save) {
        if(save == null) {
            Debug.LogError("SaveData null error");
            return;
        }

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
    private static SaveData LoadFile(string path) {

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
    private void FindAndLoadExistingSave() {
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
                SaveData saveLoaded = LoadFile(currentFile);

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

    /// <summary>
    /// Return the current SaveData object used
    /// </summary>
    /// <returns></returns>
    private SaveData GetCurrentSave() {
        return saveDataArray[currentSave];
    }

    /// <summary>
    /// Launch the capture of every saveable object data to save them and fill the SaveData object with the datas
    /// </summary>
    /// <param name="save">The SaveData we want to fill the datas in</param>
    private void CaptureState(SaveData save) {
        // Guard
        if (save == null) {
            return;
        }
        // We find every saveable entity
        foreach (var saveable in FindObjectsOfType<SaveableEntity>()) {
            // We capture the state of the objects in it and link it via the SaveableEntity ID
            save.state[saveable.Id] = saveable.CaptureState();
        }
    }

    /// <summary>
    /// Launch the restoration of every saveable object data to load them.
    /// </summary>
    /// <param name="save">The SaveData we want to get the datas from</param>
    private void RestoreState(SaveData save) {
        // Guard
        if(save == null) {
            return;
        }
        // We find every saveable entity
        foreach (var saveable in FindObjectsOfType<SaveableEntity>()) {
            // We try to get back a value out of the ID of the SaveableEntity
            if (save.state.TryGetValue(saveable.Id, out object value)) {
                // Restore the data of the objects in the saveable entity
                saveable.RestoreState(value);
            }
        }
    }

    /// <summary>
    /// Create a new save with the default parameter and save it
    /// </summary>
    /// <param name="slot">slot where you want the save to be created</param>
    public void CreateNewSave(int slot) {
        // Create new SaveData
        SaveData newSave = new SaveData();
        // Put the new SaveData in the slot passed in argument
        saveDataArray[slot] = newSave;
        // We save the new SaveData so it's in a file
        SaveFile(newSave);
    }

    /// <summary>
    /// Delet the file in the slot passed in argument
    /// </summary>
    /// <param name="slot">the slot of the file</param>
    public void DeleteSave(int slot) {
        string path = Application.persistentDataPath + "\\save_" + slot + ".dat";
        try {
            // Check if file exists with its full path    
            if (File.Exists(path)) {
                // If file found, delete it    
                File.Delete(path);
                Debug.Log("File deleted.");
            }
            else {
                Debug.LogWarning("File not found at : " + path);
            }
        }
        catch (IOException ioExp) {
            Debug.LogWarning(ioExp.Message);
        }
    }

    /// <summary>
    /// Set the current save and restore all data in it
    /// </summary>
    /// <param name="slot"></param>
    public void SetCurrentSave(int slot) {
        // Put the slot wanted as currentSlot
        currentSave = slot;
        //We check if the current slot has a SaveData object
        if (saveDataArray[currentSave] != null) {
            // We load the data in it
            Load();
            Debug.Log("Current save set in slot : " + slot);

        }
    }

    /// <summary>
    /// Save the current save this is the method that will be called by UI
    /// </summary>
    public void Save() {

        if (GetCurrentSave() == null)
        {
            CreateNewSave(currentSave);
        }

        // Path for the loading of the file
        string path = Application.persistentDataPath + "\\save_" + currentSave + ".dat";
        //saveDataArray[currentSave] = LoadFile(path);

        // We get the current save
        SaveData save = GetCurrentSave();
        // Capture all datas
        CaptureState(save);
        // Save the file
        SaveFile(save);
    }

    /// <summary>
    /// Load the current save 
    /// </summary>
    public void Load() {
        if (GetCurrentSave() == null) {
            CreateNewSave(currentSave);
        }
        // Path for the loading of the file
        string path = Application.persistentDataPath + "\\save_" + currentSave + ".dat";
        saveDataArray[currentSave] = LoadFile(path);
        // Restore all data to all saveable objects of the currentSave
        RestoreState(GetCurrentSave());
    }
}
