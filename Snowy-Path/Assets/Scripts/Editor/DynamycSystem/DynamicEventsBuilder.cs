using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

[System.Serializable]
public struct StrDynamicEvent {

    public string eventName;
}

[CreateAssetMenu(fileName = "DynamicEventsBuilder", menuName = "DynamicSystem/EventsBuilder")]
public class DynamicEventsBuilder : ScriptableObject {

    private string filePath = @"Assets\Scripts\DynamicSystem\EDynamicEvent.cs";
    public StrDynamicEvent[] dynamicEvents;

    public bool GenerateFile() {
        List<string> lines = new List<string>();
        lines.Add("[System.Serializable]");
        lines.Add("public enum EDynamicEvent {");

        for (int i = 0; i < dynamicEvents.Length; i++) {
            string eventName = dynamicEvents[i].eventName.ToString();
            eventName = Regex.Replace(eventName, @"(^\w)|(\s\w)", m => m.Value.ToUpper());
            eventName = eventName.Replace(" ", string.Empty);

            if (!Regex.IsMatch(eventName, "^[a-zA-Z0-9]*$")) {
                Debug.LogError($"{eventName} : wrong format. DynamicEvent name must be alphanumeric");
                return false;
            }

            if (dynamicEvents.Count(s => s.eventName == dynamicEvents[i].eventName) > 1) {
                Debug.LogError($"{eventName} : multiple occurencies. DynamicEvent name must be unique");
                return false;
            }

            lines.Add($"{eventName} = {i},");
        }

        lines.Add($"COUNT = {dynamicEvents.Length}");
        lines.Add("}");

        // Write the string array to a new file named "WriteLines.txt".
        using (StreamWriter outputFile = new StreamWriter(filePath)) {
            foreach (string line in lines)
                outputFile.WriteLine(line);
        }

        Debug.Log($"{dynamicEvents.Length} events generated");
        return true;
    }
}

