using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JDScopeTest : MonoBehaviour, IHandTool
{
    public void PrimaryUse() {
        Look();
    }

    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
    }

    private void Look() {
        Debug.Log("Looking far away !");
    }
}
