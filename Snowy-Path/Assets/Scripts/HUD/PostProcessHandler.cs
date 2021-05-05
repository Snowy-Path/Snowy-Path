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

    public bool progressiveDof = false;
    public float progressiveThreshold = 0.6f;

    [Header("Vignette")]
    public float vignetteMin = 0.2f;
    public float vignetteMax = 0.35f;

    private PlayerController controller;
    private DepthOfField dofComponent;
    private Vignette vignetteComponent;
    private Camera playerCamera;

    private bool inRecovery = false;
    private float targetDof;

    void Start() {
        controller = FindObjectOfType<PlayerController>();
        playerCamera = Camera.main;
        LoadProfile();
    }

    public void LoadProfile() {
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
        float vignetteStep = (vignetteMax - vignetteMin) / fovTransitionTime * Time.deltaTime;

        if (controller.IsRunning) {
            //FOV
            playerCamera.fieldOfView = Mathf.Clamp(playerCamera.fieldOfView + fovStep * 2, fovMin, fovMax);

            //DOF
            dofComponent.active = false;
            targetDof = dofMax;
            if (progressiveDof && controller.SprintTimer / controller.maxSprintDuration > progressiveThreshold) {
                dofComponent.active = true;
                dofComponent.focalLength.value = Mathf.Clamp(dofComponent.focalLength.value + dofStep / 2, 1, dofMax);

            }
            else
                dofComponent.active = true;

            //VIGNETTE
            vignetteComponent.intensity.value = Mathf.Clamp(vignetteComponent.intensity.value + vignetteStep, vignetteMin, vignetteMax);

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
            }
            else {
                dofComponent.focalLength.value = Mathf.Clamp(dofComponent.focalLength.value - dofStep, 1, dofMax);
            }

            //VIGNETTE
            vignetteComponent.intensity.value = Mathf.Clamp(vignetteComponent.intensity.value - vignetteStep, vignetteMin, vignetteMax);
        }

        //Debug.Log(dofComponent.focalLength.value);
        //Debug.Log(vignetteComponent.intensity.value);
    }
}
