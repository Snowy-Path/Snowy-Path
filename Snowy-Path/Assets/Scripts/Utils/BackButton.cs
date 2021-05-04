using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackButton : MonoBehaviour {
    public void Back() {
        var goBtns = GameObject.FindGameObjectsWithTag("BackButton");
        foreach (var go in goBtns) {
            Button btn = go.GetComponent<Button>();
            if (btn)
                btn.onClick.Invoke();
        }
    }
}
