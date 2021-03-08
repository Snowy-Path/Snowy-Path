﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class VRScope : MonoBehaviour, IHandTool {
    [Header("Set up")]
    [Tooltip("Hands animator. Allows this script to trigger the look animation.")]
    [SerializeField] Animator animator;
    [SerializeField] Camera scopeCamera;
    [SerializeField] GameObject spriteMask;

    [Header("Zoom")]
    [SerializeField] float defaultZoom = 4f;
    [SerializeField] float secondaryZoom = 8f;

    public EToolType ToolType => EToolType.Scope;

    public bool IsBusy { get; set; }

    private void Start() {
        spriteMask.SetActive(false);
    }

    private void Update() {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {

        }
    }

    public void StartPrimaryUse() {
        IsBusy = true;
        scopeCamera.fieldOfView = defaultZoom;

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            animator.SetBool("LookInTelescope", true);
        }
    }

    public void CancelPrimaryUse() {
        IsBusy = false;
        animator.SetBool("LookInTelescope", false);
    }

    public void SecondaryUse() {
        if (scopeCamera.fieldOfView == defaultZoom)
            scopeCamera.fieldOfView = secondaryZoom;
        else
            scopeCamera.fieldOfView = defaultZoom;
    }

    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
        spriteMask.SetActive(false);
    }
}
