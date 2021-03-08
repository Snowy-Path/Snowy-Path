using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDebug : MonoBehaviour {

    private void OnGUI() {

        WolfController[] wolwes = FindObjectsOfType<WolfController>();

        GUI.Box(new Rect(Screen.width - 200, 10, 190, 30 + 30 * wolwes.Length), "Current state");

        for (int i = 0; i < wolwes.Length; i++) {
            GUI.Label(new Rect(Screen.width - 190, 40 + 30*i, 180, 30 + 30 * wolwes.Length), $"{wolwes[i].name} : {wolwes[i].GetCurrentState()}");
        }

    }

}
