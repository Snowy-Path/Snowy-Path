using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JDPistolTest : MonoBehaviour, IHandTool {
    public EToolType ToolType { get => EToolType.Pistol; }

    public void StartPrimaryUse() {
        Fire();
    }

    public void CancelPrimaryUse() {
        //Cancel is not possible on pistol
    }

    public void SecondaryUse() {
        Debug.Log("Nothing");
    }

    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
    }

    private void Fire() {
        Debug.Log("PAN PAN !");
    }

}
