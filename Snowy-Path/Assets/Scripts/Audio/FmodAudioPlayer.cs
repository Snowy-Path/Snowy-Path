using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FmodAudioPlayer : MonoBehaviour {
    [FMODUnity.EventRef]
    [SerializeField] string soundPath = "";
    [SerializeField] float paramValue;

    private FMOD.Studio.EventInstance instance;
    private FMOD.Studio.PARAMETER_ID paramId;

    public void SetParam(float param) {
        paramValue = param;
    }

    public void PlaySound() {
        instance = FMODUnity.RuntimeManager.CreateInstance(soundPath);
        instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));

        FMOD.Studio.EventDescription paramEventDesc = FMODUnity.RuntimeManager.GetEventDescription(soundPath);
        FMOD.Studio.PARAMETER_DESCRIPTION paramDesc;

        paramEventDesc.getParameterDescriptionCount(out int paramCount);

        if (paramCount > 0) {
            paramEventDesc.getParameterDescriptionByIndex(0, out paramDesc);
            paramId = paramDesc.id;
            instance.setParameterByID(paramId, paramValue);
        }

        instance.start();
        instance.release();
    }
}
