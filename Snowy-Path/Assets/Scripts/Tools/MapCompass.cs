using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCompass : MonoBehaviour, IHandTool {
    public EToolType ToolType => EToolType.MapCompass;

    public void CancelPrimaryUse() {
        
    }

    public void SecondaryUse() {
        
    }

    public void StartPrimaryUse() {
        
    }

    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
    }
}
