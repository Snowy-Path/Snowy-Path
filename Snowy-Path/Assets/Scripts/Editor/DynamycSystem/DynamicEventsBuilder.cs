using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEditor;

[System.Serializable]
public struct StrDynamicEvent {
    public int id;
    public string eventName;
}

[CreateAssetMenu(fileName = "DynamicEventsBuilder", menuName = "DynamicSystem/EventsBuilder")]
public class DynamicEventsBuilder : ScriptableObject {

    //List of all dynamic events referenced in inspector
    public List<StrDynamicEvent> dynamicEvents;

    private string filePath = @"Assets\Scripts\DynamicSystem\EDynamicEvent.cs";
    private int lastIndex = -1;

    /// <summary>
    /// Create a file containing the enums coresponding to referenced events
    /// </summary>
    /// <returns></returns>
    public bool GenerateFile() {
        //Init list of lines to write in file
        List<string> lines = new List<string>();
        
        //Add head of file
        lines.Add("[System.Serializable]");
        lines.Add("public enum EDynamicEvent {");

        //Write each referenced dynamic event
        for (int i = 0; i < dynamicEvents.Count; i++) {

            //Get event name and format it : set first letters in words to upper case and remove white spaces
            string eventName = dynamicEvents[i].eventName;
            eventName = Regex.Replace(eventName, @"(^\w)|(\s\w)", m => m.Value.ToUpper());
            eventName = eventName.Replace(" ", string.Empty);

            //Check if name is only alpha numeric
            if (!Regex.IsMatch(eventName, "^[a-zA-Z0-9]*$")) {
                Debug.LogError($"{eventName} : wrong format. DynamicEvent name must be alphanumeric");
                return false;
            }

            //Check if there are multiple occurencies of the same event
            if (dynamicEvents.Count(s => s.eventName == dynamicEvents[i].eventName) > 1) {
                Debug.LogError($"{eventName} : multiple occurencies. DynamicEvent name must be unique");
                return false;
            }

            //Save event name and id in string
            string line = $"{eventName} = {dynamicEvents[i].id}";
            if (i < dynamicEvents.Count - 1)
                line += ",";

            //Add string to list
            lines.Add(line);
        }

        //End file
        lines.Add("}");

        //Write all lines in file
        using (StreamWriter outputFile = new StreamWriter(filePath)) {
            foreach (string line in lines)
                outputFile.WriteLine(line);
        }
        //Reload generated file
        AssetDatabase.ImportAsset(filePath);

        //Feedback
        Debug.Log($"{dynamicEvents.Count} events generated");

        return true;
    }

    /// <summary>
    /// Clear referenced events
    /// </summary>
    public void Reset() {
        dynamicEvents.Clear();
        lastIndex = -1;
    }

    /// <summary>
    /// Add event to list
    /// </summary>
    public void AddItem() {
        lastIndex++;
        dynamicEvents.Add(new StrDynamicEvent() { eventName = "NewEvent", id = lastIndex });
    }
}

