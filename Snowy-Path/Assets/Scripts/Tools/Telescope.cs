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
    [SerializeField] float zoomStep = 0.05f;

    [Header("Audio")]
    public UnityEvent OnZoom;
    public UnityEvent OnEquip;

    public EToolType ToolType => EToolType.Scope;

    public bool IsBusy { get; set; }
    public Animator handAnimator { get; set; }

    private float targetFov;
    private bool inZooming = false;

    private void Start() {
        targetFov = defaultZoom;
        spriteMask.SetActive(false);
        scopeCamera.gameObject.SetActive(false);
        lens.SetActive(false);
    }

    private void Update() {

        if (targetFov + .1f < scopeCamera.fieldOfView || scopeCamera.fieldOfView < targetFov - .1f) {
            inZooming = true;
            scopeCamera.fieldOfView = Mathf.Lerp(scopeCamera.fieldOfView, targetFov, zoomStep);
        }
        else {
            scopeCamera.fieldOfView = targetFov;
            inZooming = false;
        }
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
    }

    public void SwitchZoom() {
        if (inZooming)
            return;

        if (IsBusy && gameObject.activeSelf) {
            if (targetFov == defaultZoom)
                targetFov = secondaryZoom;
            else
                targetFov = defaultZoom;
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
