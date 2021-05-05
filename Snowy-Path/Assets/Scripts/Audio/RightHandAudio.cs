using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RightHandAudio : MonoBehaviour {

    private FMOD.Studio.EventInstance instance;
    private FMOD.Studio.PARAMETER_ID soilParamId;

    public UnityEvent torchAttack;
    public UnityEvent step;
    public UnityEvent death;

    public void AudioPlayTorchAttack() {
        torchAttack.Invoke();
    }
    public void AudioPlayWalk(AnimationEvent evt) {
        if (evt.animatorClipInfo.weight > 0.1f) {
            step.Invoke();
            Debug.Log("Right");
        }
        //FootStepSound(0); //TODO : replace number by ground type
    }

    public void AudioPlayRun(AnimationEvent evt) {
        if (evt.animatorClipInfo.weight > 0.1f)
            step.Invoke();
    }

    public void AudioPlayDeath(AnimationEvent evt) {
        death.Invoke();
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
