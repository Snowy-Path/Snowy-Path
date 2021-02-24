using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(DynamicEventsBuilder))]
public class EventsBuilderEditor : Editor {

    private SerializedProperty dynamicEvents;
    private ReorderableList list;
    private DynamicEventsBuilder builder;

    private void OnEnable() {
        //Init fields
        dynamicEvents = serializedObject.FindProperty("dynamicEvents");
        builder = (DynamicEventsBuilder)target;
        list = new ReorderableList(serializedObject, dynamicEvents, true, true, true, true);

        //Init callbacks
        list.drawElementCallback = DrawListItems;
        list.drawHeaderCallback = DrawHeader;
        list.onAddCallback += AddItem;
    }

    public override void OnInspectorGUI() {

        serializedObject.Update();  //Update builder dynamicEvents list
        list.DoLayoutList();        //Render reordable list
        serializedObject.ApplyModifiedProperties(); //Update builder properties

        //Add file generation button in inspector and handle click
        if (GUILayout.Button("Generate Dynamic Events")) {
            Debug.Log("Starting events file generation...");
            if (builder.GenerateFile()) {
                Debug.Log("Events generation done.");
            }
            else {
                Debug.LogError("Events file generation failed.");
            }
        }

        //Create reset button style
        var style = new GUIStyle(GUI.skin.button);
        style.normal.textColor = Color.red;

        //Create reset button and handle click
        if (GUILayout.Button("Reset", style, GUILayout.ExpandWidth(false), GUILayout.Width(80))) {
            //Show confirmation dialog
            EditorUtility.DisplayDialog("Reset Dynamic Events", "Are you sure you want to reset all dynamic events?", "Yes", "No");
            {
                builder.Reset();
            }
        }
    }

    /// <summary>
    /// Draw event list item
    /// </summary>
    /// <param name="rect">Rect display</param>
    /// <param name="index">index of event in list</param>
    /// <param name="isActive"></param>
    /// <param name="isFocused"></param>
    private void DrawListItems(Rect rect, int index, bool isActive, bool isFocused) {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index); //The element in list

        EditorGUI.LabelField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), "Name"); //Item name label

        //Add event name column
        EditorGUI.PropertyField(
            new Rect(rect.x + 60, rect.y, 100, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("eventName"),
            GUIContent.none
        );

        //Add id column
        EditorGUI.LabelField(new Rect(rect.x + 180, rect.y, 40, EditorGUIUtility.singleLineHeight), $"Id : {builder.dynamicEvents[index].id}");
    }

    /// <summary>
    /// Add header to inspector view
    /// </summary>
    /// <param name="rect"></param>
    private void DrawHeader(Rect rect) {
        string name = "Dynamic Events";
        EditorGUI.LabelField(rect, name);
    }

    /// <summary>
    /// Call builder method AddItem()
    /// </summary>
    /// <param name="reorderList">Rect display</param>
    private void AddItem(ReorderableList reorderList) {
        builder.AddItem();
    }
}
