using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Telescope : MonoBehaviour
{

    public GameObject scopeOverlay; //Overlay effect on UI
    public GameObject scopeCamera;
    public Camera MainCamera;
    public float magnification1;
    public float magnification2;
    private float normalFOV; //Basic Camera FOV
    private bool isScoped;
    private void Start()
    {
        scopeCamera.SetActive(true);
        scopeCamera.SetActive(false);
    }
    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard.eKey.wasPressedThisFrame)
        {
            MainInteraction();
        }
        //If in scope mode, change FOV
        if (keyboard.fKey.wasPressedThisFrame && isScoped)
        {
            SecondaryInteraction();
        }
    }
    /// <summary>
    /// If unscoped, active scope mode, else disactive it
    /// </summary>
    void MainInteraction()
    {
        isScoped = !isScoped;
        if (isScoped)
        {
            StartCoroutine(OnScoped());

        }
        else
        {
            OnUnscoped();

        }

    }
    /// <summary>
    /// Change FOV to zoom further
    /// </summary>
    void SecondaryInteraction()
    {
        MainCamera.fieldOfView = magnification2;
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
