
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DynamicEvent))] // 'Test' here is your component class name
public class DynamicEventEditor : Editor {

    public override void OnInspectorGUI() {
        DynamicEvent test = target as DynamicEvent;
        EditorGUILayout.LabelField("description");
        test.description = EditorGUILayout.TextArea(test.description);
    }
}
