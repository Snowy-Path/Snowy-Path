using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JDScopeTest : MonoBehaviour, IHandTool
{
    public EToolType ToolType { get => EToolType.Scope; }

    public void StartPrimaryUse() {
        Look();
    }

    public void CancelPrimaryUse() {
        Debug.Log("Stop using scope");
    }

    public void SecondaryUse() {
        Debug.Log("Apply x4 zoom");
    }

    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
    }

    private void Look() {
        Debug.Log("Looking far away !");
    }
}
