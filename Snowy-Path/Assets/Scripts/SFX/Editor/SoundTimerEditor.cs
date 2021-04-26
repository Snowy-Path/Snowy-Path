using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundTimer))]
[CanEditMultipleObjects]
public class SoundTimerEditor : Editor {

    SerializedProperty m_eventPath;

    SerializedProperty m_randomCooldown;

    SerializedProperty m_cooldown;

    SerializedProperty m_cooldownMin;
    SerializedProperty m_cooldownMax;

    void OnEnable() {
        m_eventPath = serializedObject.FindProperty("m_eventPath");

        m_randomCooldown = serializedObject.FindProperty("m_randomCooldown");

        m_cooldown = serializedObject.FindProperty("m_cooldown");

        m_cooldownMin = serializedObject.FindProperty("m_cooldownMin");
        m_cooldownMax = serializedObject.FindProperty("m_cooldownMax");
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.PropertyField(m_eventPath);

        bool toggle = EditorGUILayout.Toggle("Random Cooldown ?", m_randomCooldown.boolValue);
        if (toggle != m_randomCooldown.boolValue) {
            m_randomCooldown.boolValue = toggle;
        }

        if (!toggle) {
            EditorGUILayout.PropertyField(m_cooldown);
        } else {
            EditorGUILayout.PropertyField(m_cooldownMin);
            EditorGUILayout.PropertyField(m_cooldownMax);
        }

        serializedObject.ApplyModifiedProperties();
    }
}