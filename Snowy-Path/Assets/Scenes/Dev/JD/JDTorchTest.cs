using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JDTorchTest : MonoBehaviour, IHandTool {

    public EToolType ToolType { get => EToolType.Torch; }

    public GameObject fireFX;

    public void StartPrimaryUse() {
        ToggleFire();
    }
   public void CancelPrimaryUse() {
    }

    public void SecondaryUse() {
    }

    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
    }

    private void ToggleFire() {
        fireFX.SetActive(!fireFX.activeSelf);
    }

 
}
