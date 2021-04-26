using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SFX : MonoBehaviour {

    #region Variables
    #region Wind
    //[SerializeField]
    //private FMODUnity.StudioEventEmitter m_windPlainsEmitter;

    [FMODUnity.EventRef]
    public string m_windPlainsEvent = "";
    private FMOD.Studio.EventInstance m_windPlainsInstance;

    [SerializeField]
    private string m_windPlainsTag;

    private FMOD.Studio.PARAMETER_ID m_windPlainsID;
    #endregion

    //#region WaterDrops
    //[SerializeField]
    //private FMODUnity.StudioEventEmitter m_waterCaveDropEmitter;

    //[SerializeField]
    //private string m_waterCaveDropTag;

    //private FMOD.Studio.PARAMETER_ID m_waterCaveDropID;
    //#endregion
    #endregion

    private void Start() {

        m_windPlainsInstance = FMODUnity.RuntimeManager.CreateInstance(m_windPlainsEvent);

        // Wind Plains parameter
        FMOD.Studio.EventDescription windPlainsEventDesc;
        m_windPlainsInstance.getDescription(out windPlainsEventDesc);
        FMOD.Studio.PARAMETER_DESCRIPTION windPlainsParametterDesc;
        windPlainsEventDesc.getParameterDescriptionByIndex(0, out windPlainsParametterDesc);
        m_windPlainsID = windPlainsParametterDesc.id;

        // Water Cave Drop parameter
        //FMOD.Studio.EventDescription waterCaveDropEventDesc;
        //m_waterCaveDropEmitter.EventInstance.getDescription(out waterCaveDropEventDesc);
        //FMOD.Studio.PARAMETER_DESCRIPTION waterCaveDropParametterDesc;
        //waterCaveDropEventDesc.getParameterDescriptionByIndex(0, out waterCaveDropParametterDesc);
        //m_waterCaveDropID = waterCaveDropParametterDesc.id;
    }

    private void Update() {
        float value;
        m_windPlainsInstance.getParameterByID(m_windPlainsID, out value);
        Debug.Log(value);
    }

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag(m_windPlainsTag)) {
            StartWind();
        }
        //else if (other.CompareTag(m_waterCaveDropTag)) {
        //    StartWater();
        //}
    }

    private void OnTriggerExit(Collider other) {

        if (other.CompareTag(m_windPlainsTag)) {
            StopWind();
        }
        //else if (other.CompareTag(m_waterCaveDropTag)) {
        //    StopWater();
        //}
    }


    private void StartWind() {
        Debug.Log("START WIND");

        FMOD.Studio.PLAYBACK_STATE playbackState;
        m_windPlainsInstance.getPlaybackState(out playbackState);

        if (playbackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
            Debug.Log("HERE BABY");
            //m_windPlainsEmitter.Play();
            m_windPlainsInstance.setParameterByID(m_windPlainsID, 0.0f);
            m_windPlainsInstance.start();
        }
    }

    private void StopWind() {
        Debug.Log("STOP WIND");
        m_windPlainsInstance.setParameterByID(m_windPlainsID, 1.0f);
    }


    //private void StartWater() {
    //    Debug.Log("START WATER");
    //    if (!m_waterCaveDropEmitter.IsPlaying()) {
    //        //m_waterCaveDropEmitter.Play();
    //        m_waterCaveDropEmitter.SetParameter(m_waterCaveDropID, 0.0f);
    //    }
    //}

    //private void StopWater() {
    //    Debug.Log("STOP WATER");
    //    m_waterCaveDropEmitter.SetParameter(m_waterCaveDropID, 1.0f);
    //}

}
