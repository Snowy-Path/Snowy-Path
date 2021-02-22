using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JDCompassTest : MonoBehaviour, IHandTool {

    public void PrimaryUse() {
        Locate();
    }
    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
    }

    private void Locate() {
        Debug.Log("Youre here !");
    }

    
}
