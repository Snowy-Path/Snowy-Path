using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(DynamicEventsBuilder))]
public class EventsBuilderEditor : Editor {

    SerializedProperty dynamicEvents;
    ReorderableList list;

    private void OnEnable() {
        dynamicEvents = serializedObject.FindProperty("dynamicEvents");

        list = new ReorderableList(serializedObject, dynamicEvents, true, true, true, true);

        list.drawElementCallback = DrawListItems;
        list.drawHeaderCallback = DrawHeader;
    }

    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused) {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index); //The element in the list

        EditorGUI.LabelField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), $"Event {index}");

        EditorGUI.PropertyField(
            new Rect(rect.x + 60, rect.y, 100, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("eventName"),
            GUIContent.none
        );
    }

    void DrawHeader(Rect rect) {
        string name = "Dynamic Events";
        EditorGUI.LabelField(rect, name);
    }

    public override void OnInspectorGUI() {

        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Generate Dynamic Events")) {
            DynamicEventsBuilder builder = (DynamicEventsBuilder)target;
            Debug.Log("Starting events file generation...");
            if (builder.GenerateFile()) {
                Debug.Log("Events generation done.");
            }
            else {
                Debug.LogWarning("Events file generation failed.");
            }
        }
    }
}
