using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveableEntity : MonoBehaviour
{
    // The ID of the Saveable entity that will link this object with the data saved
    [SerializeField] private string id = string.Empty;

    public string Id => id;

    // The ID must be generated for it to be saved properly
    [ContextMenu("Generate Id")]
    private void GenerateId() => id = Guid.NewGuid().ToString();

    /// <summary>
    /// Capture all datas that has the ISaveable implemented in the same component
    /// </summary>
    /// <returns>A dictionary of string and objects</returns>
    public object CaptureState() {
        // The return dictionary
        var state = new Dictionary<string, object>();
        // We get all scripts that implement the Isaveable interface
        foreach (var saveable in GetComponents<ISaveable>()) {
            // Save the data of this script in the dictionary with the name of the scripts as key
            state[saveable.GetType().ToString()] = saveable.CaptureState();
        }

        return state;
    }

    /// <summary>
    /// Restore all datas that has the ISaveable implemented in the same component
    /// </summary>
    /// <param name="state">The dictionary with the datas</param>
    public void RestoreState(object state) {
        // We cast the object we got back from the SaveSystem
        var stateDictionary = (Dictionary<string, object>)state;

        // We get all scripts that implement the Isaveable interface
        foreach (var saveable in GetComponents<ISaveable>()) {
            // We get the typename from the data to link it in the dictionary
            string typeName = saveable.GetType().ToString();
            if(stateDictionary.TryGetValue(typeName, out object value)) {
                // If we found a data linked with the typename
                // we send the datas to the object that will restore himself
                saveable.RestoreState(value);
            }

        }
    }
}
