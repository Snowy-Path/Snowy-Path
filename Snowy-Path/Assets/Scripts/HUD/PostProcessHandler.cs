using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessHandler : MonoBehaviour {

    [Header("FOV")]
    public float fovMin = 60f;
    public float fovMax = 65f;
    [Tooltip("Transition time to max FOV to min FOV")]
    public float fovTransitionTime = 3f;

    [Header("DOF")]
    public float dofMin = 10f;
    public float dofMax = 15f;
    [Tooltip("Time to fade from max DOF to 0")]
    public float dofRecoveryTime = 3f;
    [Tooltip("Recovery DOF speed variation")]
    public float dofSpeed = 5;
    //public AnimationCurve dofTransitionCurve;

    private PlayerController controller;
    private DepthOfField dofComponent;
    private Vignette vignetteComponent;
    private Camera playerCamera;

    private float dofAxis;
    private bool inRecovery = false;
    private float targetDof;

    void Start() {
        controller = FindObjectOfType<PlayerController>();
        playerCamera = Camera.main;
        Volume volume = GetComponent<Volume>();
        DepthOfField tmpDoF;
        if (volume.profile.TryGet<DepthOfField>(out tmpDoF)) {
            dofComponent = tmpDoF;
        }
        volume.profile.TryGet<Vignette>(out vignetteComponent);
    }



    void Update() {
        float fovStep = (fovMax - fovMin) / fovTransitionTime * Time.deltaTime;
        float dofStep = dofMax / dofRecoveryTime * Time.deltaTime;
        if (controller.IsRunning) {
            //FOV
            playerCamera.fieldOfView = Mathf.Clamp(playerCamera.fieldOfView + fovStep*2, fovMin, fovMax);
            dofComponent.active = false;
            dofAxis = 0;
            targetDof = dofMax;
        }
        else {
            inRecovery = !controller.IsRunning && controller.SprintTimer > 0;

            //FOV
            playerCamera.fieldOfView = Mathf.Clamp(playerCamera.fieldOfView - fovStep, fovMin, fovMax);

            //DOF
            if (inRecovery) {
                dofComponent.active = true;
                if (Mathf.Abs(dofComponent.focalLength.value - targetDof) < 0.1f)
                    targetDof = Random.Range(dofMin, dofMax);
                dofComponent.focalLength.value = Mathf.Lerp(dofComponent.focalLength.value, targetDof, Time.deltaTime * dofSpeed);
                Debug.Log(dofComponent.focalLength.value);
            }
            else {
                dofComponent.focalLength.value = Mathf.Clamp(dofComponent.focalLength.value - dofStep, 1, dofMax);
            }
        }
    }
}
