using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsAudio : MonoBehaviour {

    [SerializeField] string torchAttackPath = "";
    [SerializeField] string stepPath = "";

    public void PlayTorchAttack() {
        FMODUnity.RuntimeManager.PlayOneShot(torchAttackPath, transform.position);
    }

    public void PlayWalk() {
        FootStepSound(0);
    }

    public void PlayRun() {
        FootStepSound(1);
    }

    private FMOD.Studio.EventInstance instance;
    private FMOD.Studio.PARAMETER_ID soilParamId;
    private void FootStepSound(float soil) {
        instance = FMODUnity.RuntimeManager.CreateInstance(stepPath);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

        FMOD.Studio.EventDescription soilEventDesc = FMODUnity.RuntimeManager.GetEventDescription(stepPath);
        FMOD.Studio.PARAMETER_DESCRIPTION soilParamDesc;
        soilEventDesc.getParameterDescriptionByName("Kind of soil", out soilParamDesc);
        soilParamId = soilParamDesc.id;

        instance.setParameterByID(soilParamId, soil);
        instance.start();
        instance.release();
    }
}
