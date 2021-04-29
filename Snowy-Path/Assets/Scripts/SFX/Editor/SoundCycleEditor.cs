using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundCycle))]
[CanEditMultipleObjects]
public class SoundCycleEditor : Editor {

    SerializedProperty m_hasDelay;
    SerializedProperty m_startDelay;
    SerializedProperty m_cycleDelay;

    private void OnEnable() {
        m_hasDelay = serializedObject.FindProperty("m_hasDelay");
        m_startDelay = serializedObject.FindProperty("m_startDelay");
        m_cycleDelay = serializedObject.FindProperty("m_cycleDelay");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        bool toggle = EditorGUILayout.Toggle("Has Delay ?", m_hasDelay.boolValue);
        if (toggle != m_hasDelay.boolValue) {
            m_hasDelay.boolValue = toggle;
        }

        if (toggle) {
            EditorGUILayout.PropertyField(m_startDelay);
        }

        EditorGUILayout.PropertyField(m_cycleDelay);

        serializedObject.ApplyModifiedProperties();
    }

}
