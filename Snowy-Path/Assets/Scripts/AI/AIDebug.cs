using UnityEngine;

/// <summary>
/// Prints on screen the current state of every WolfController in the scene.
/// </summary>
public class AIDebug : MonoBehaviour {

    /// <summary>
    /// Retrieves every activated WolfController and prints their current state on screen using the GUI feature.
    /// Called at each frame.
    /// </summary>
    private void OnGUI() {

        WolfController[] wolwes = FindObjectsOfType<WolfController>();

        GUI.Box(new Rect(Screen.width - 200, 10, 190, 30 + 30 * wolwes.Length), "Current state");

        for (int i = 0; i < wolwes.Length; i++) {
            GUI.Label(new Rect(Screen.width - 190, 40 + 30*i, 180, 30 + 30 * wolwes.Length), $"{wolwes[i].name} : {wolwes[i].GetCurrentState()}");
        }

    }

}
