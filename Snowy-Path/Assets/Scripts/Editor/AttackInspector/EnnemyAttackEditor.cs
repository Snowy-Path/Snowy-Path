using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


[CustomEditor(typeof(EnnemyAttack))]
public class EnnemyAttackEditor : Editor {
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
