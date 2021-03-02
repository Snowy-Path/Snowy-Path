using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Telescope : MonoBehaviour, IHandTool
{

    public GameObject scopeOverlay; //Overlay effect on UI
    public GameObject scopeCamera;
    public Camera MainCamera;
    public float magnification1;
    public float magnification2;
    private float normalFOV; //Basic Camera FOV
    private bool isScoped;

    public EToolType ToolType =>EToolType.Scope;

    private void Start()
    {
        scopeOverlay.SetActive(true);
        scopeCamera.SetActive(false);
        scopeOverlay.SetActive(false);
        scopeCamera.SetActive(true);
        //MainCamera.fieldOfView = normalFOV;

    }

    public void StartPrimaryUse() {
        isScoped = !isScoped;
        if (isScoped) {
            StartCoroutine(OnScoped());

        }
        else {
            OnUnscoped();

        }
    }

    public void CancelPrimaryUse() {
        // Nothing
    }

    public void SecondaryUse() {
        MainCamera.fieldOfView = magnification2;
    }

    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
    }

    /// <summary>
    /// Change FOV back to normal and disable Overlay
    /// </summary>
    void OnUnscoped()
    {
        //
        scopeOverlay.SetActive(false);
        scopeCamera.SetActive(true);
        MainCamera.fieldOfView = normalFOV;

    }

    /// <summary>
    /// Change
    /// </summary>
    /// <param name="maglevel"></param>
    /// <returns></returns>
    IEnumerator OnScoped()
    {
        yield return new WaitForSeconds(.15f);
        //Activate overlay and "Vision" Layer objects
        scopeOverlay.SetActive(true);
        scopeCamera.SetActive(false);
        //Change FOV
        normalFOV = MainCamera.fieldOfView;
        MainCamera.fieldOfView = magnification1;

    }


}


