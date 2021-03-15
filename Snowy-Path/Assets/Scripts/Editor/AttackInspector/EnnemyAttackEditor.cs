using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Inspector Editor script to manage EnnemyAttack script parameters in inspector.
/// </summary>
[CustomEditor(typeof(EnnemyAttack))]
public class EnnemyAttackEditor : Editor {

    /// <summary>
    /// Manage EnnemyAttack script parameters. If the <c>instantKill</c> parameter is enabled, this method hides the <c>attackDamage</c> parameter.
    /// </summary>
    public override void OnInspectorGUI() {

        var myScript = target as EnnemyAttack;

        myScript.instantKill = GUILayout.Toggle(myScript.instantKill, "Instant kill");

        if (!myScript.instantKill) {
            myScript.attackDamage = EditorGUILayout.IntSlider("Attack damage:", myScript.attackDamage, 1, 100);
        }

        myScript.temperaturePercentageDamage = EditorGUILayout.Slider("Temperature %", myScript.temperaturePercentageDamage, 0, 1);
        myScript.clothPercentageDamage = EditorGUILayout.Slider("Cloth %", myScript.clothPercentageDamage, 0, 1);

        if (GUI.changed && !Application.isPlaying) {
            EditorUtility.SetDirty(target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

    }
}
