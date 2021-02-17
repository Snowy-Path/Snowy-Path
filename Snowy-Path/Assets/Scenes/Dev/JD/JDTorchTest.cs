using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JDTorchTest : MonoBehaviour, IHandTool {

    public GameObject fireFX;

    public void PrimaryUse() {
        ToggleFire();
    }

    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
    }

    private void ToggleFire() {
        fireFX.SetActive(!fireFX.activeSelf);
    }
}
