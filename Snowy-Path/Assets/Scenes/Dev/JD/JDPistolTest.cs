using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JDPistolTest : MonoBehaviour, IHandTool {

    public void PrimaryUse() {
        Fire();
    }

    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
    }

    private void Fire() {
        Debug.Log("PAN PAN !");
    }

}
