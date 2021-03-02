using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JDCompassTest : MonoBehaviour, IHandTool {
    public EToolType ToolType { get => EToolType.MapCompass; }

    public void StartPrimaryUse() {
        Locate();
    } 
    
    public void CancelPrimaryUse() {
        Debug.Log("Stop using map and compass");
    }

    public void SecondaryUse() {
        Debug.Log("Start editing mode");
    }

    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
    }

    private void Locate() {
        Debug.Log("Youre here !");
    }

    
}
