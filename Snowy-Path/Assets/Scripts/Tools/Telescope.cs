using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Telescope : MonoBehaviour, IHandTool {
    [Header("Set up")]
    [Tooltip("Hands animator. Allows this script to trigger the look animation.")]
    [SerializeField] Camera scopeCamera;
    [SerializeField] GameObject spriteMask;

    [Header("Zoom")]
    [SerializeField] float defaultZoom = 4f;
    [SerializeField] float secondaryZoom = 8f;

    public EToolType ToolType => EToolType.Scope;

    public bool IsBusy { get; set; }
    public Animator handAnimator { get; set; }

    private void Start() {
        spriteMask.SetActive(false);
        scopeCamera.gameObject.SetActive(false);
    }

    public void StartPrimaryUse() {
        IsBusy = true;
        scopeCamera.gameObject.SetActive(true);
        scopeCamera.fieldOfView = defaultZoom;

        handAnimator.SetBool("UseTelescope", true);
    }

    public void CancelPrimaryUse() {
        IsBusy = false;
        handAnimator.SetBool("UseTelescope", false);
        DisableCamera(.5f);
    }

    public IEnumerator DisableCamera(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        scopeCamera.gameObject.SetActive(false);
    }

    public void SecondaryUse() {
    }

    public void SwitchZoom() {
        if (IsBusy && gameObject.activeSelf) {
            if (scopeCamera.fieldOfView == defaultZoom)
                scopeCamera.fieldOfView = secondaryZoom;
            else
                scopeCamera.fieldOfView = defaultZoom;
        }
    }

    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
        spriteMask.SetActive(false);
    }
}
