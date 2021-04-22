using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandsAudio : MonoBehaviour {

    //[SerializeField] string torchAttackPath = "";
    //[SerializeField] string stepPath = "";

    private FMOD.Studio.EventInstance instance;
    private FMOD.Studio.PARAMETER_ID soilParamId;

    public UnityEvent torchAttack;
    public UnityEvent step;
    public UnityEvent reload;

    public void PlayTorchAttack() {
        torchAttack.Invoke();
        //FMODUnity.RuntimeManager.PlayOneShot(torchAttackPath);
    }

    public void PlayWalk(AnimationEvent evt) {
        if (evt.animatorClipInfo.weight > 0.1f)
            step.Invoke();
            //FootStepSound(0); //TODO : replace number by ground type
    }

    public void PlayRun(AnimationEvent evt) {
        if (evt.animatorClipInfo.weight > 0.1f)
            step.Invoke();
    }

    public void PlayReload(AnimationEvent evt) {
            reload.Invoke();
    }

    //private void FootStepSound(float soil) {
    //    instance = FMODUnity.RuntimeManager.CreateInstance(stepPath);
    //    instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

    //    FMOD.Studio.EventDescription soilEventDesc = FMODUnity.RuntimeManager.GetEventDescription(stepPath);
    //    FMOD.Studio.PARAMETER_DESCRIPTION soilParamDesc;
    //    soilEventDesc.getParameterDescriptionByName("Kind of soil", out soilParamDesc);
    //    soilParamId = soilParamDesc.id;

    //    instance.setParameterByID(soilParamId, soil);
    //    instance.start();
    //    instance.release();
    //}
}
