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
    [SerializeField] GameObject lens;

    [Header("Zoom")]
    [SerializeField] float defaultZoom = 4f;
    [SerializeField] float secondaryZoom = 8f;


    [Header("Audio")]
    public UnityEvent OnZoom;
    public UnityEvent OnEquip;

    public EToolType ToolType => EToolType.Scope;

    public bool IsBusy { get; set; }
    public Animator handAnimator { get; set; }

    private void Start() {
        spriteMask.SetActive(false);
        scopeCamera.gameObject.SetActive(false);
        lens.SetActive(false);
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
        lens.SetActive(false);

    }

    public IEnumerator DisableCamera(float waitTime) {
        yield return new WaitForSeconds(waitTime);
        scopeCamera.gameObject.SetActive(false);
    }

    public void SecondaryUse() {
        SwitchZoom();
    }

    private void SwitchZoom() {
        if (IsBusy && gameObject.activeSelf) {
            if (scopeCamera.fieldOfView == defaultZoom)
                scopeCamera.fieldOfView = secondaryZoom;
            else
                scopeCamera.fieldOfView = defaultZoom;
            OnZoom.Invoke();
        }
    }

    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
        spriteMask.SetActive(false);
        if (display) {
            OnEquip.Invoke();
        }
    }
}
