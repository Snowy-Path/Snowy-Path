using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FootStepsAudio : MonoBehaviour {
    [SerializeField] float soundStepFactor = 1f;
    [SerializeField] string stepPath = "";

    private float velocity = 1f;
    float timer = 0;

    public void SetParam(float param) {
        velocity = param;
    }

    void Update() {
        timer += Time.deltaTime;
        if (velocity >= 0.2f && timer >= 1 / velocity * soundStepFactor) {
            Debug.Log(1 / velocity * soundStepFactor);
            FootStepSound();
            timer = 0;
        }
    }

    private FMOD.Studio.EventInstance instance;
    private FMOD.Studio.PARAMETER_ID healthParameterId, soilParamId;
    public void FootStepSound() {
        instance = FMODUnity.RuntimeManager.CreateInstance(stepPath);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

        FMOD.Studio.EventDescription soilEventDesc = FMODUnity.RuntimeManager.GetEventDescription(stepPath);
        FMOD.Studio.PARAMETER_DESCRIPTION soilParamDesc;
        soilEventDesc.getParameterDescriptionByName("Kind of soil", out soilParamDesc);
        soilParamId = soilParamDesc.id;

        instance.setParameterByID(soilParamId, 0);
        instance.start();
        instance.release();
    }
}
