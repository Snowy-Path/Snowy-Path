using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TreeNavMeshHelper))]
public class TreeNavMeshHelperEditor : Editor {


    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        TreeNavMeshHelper helper = target as TreeNavMeshHelper;

        if (GUILayout.Button("Generate")) {
            helper.Generate();
        }

        if (GUILayout.Button("Clear")) {
            helper.Clear();
        }
    }
}
