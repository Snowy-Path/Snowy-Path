using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_SFX : MonoBehaviour {

    #region Variables
    #region Wind
    [Header("Wind Plains")]
    [FMODUnity.EventRef]
    public string m_windPlainsEvent = "";
    private FMOD.Studio.EventInstance m_windPlainsInstance;

    [SerializeField]
    private string m_windPlainsTag;

    private FMOD.Studio.PARAMETER_ID m_windPlainsID;
    private FMOD.Studio.PLAYBACK_STATE m_windPlainsPlaybackState;
    //private bool m_windIsStarting = false;
    #endregion

    #region Water
    [Header("Water Cave Drops")]
    [FMODUnity.EventRef]
    public string m_waterCaveDropEvent = "";
    private FMOD.Studio.EventInstance m_waterCaveDropInstance;

    [SerializeField]
    private string m_waterCaveDropTag;

    private FMOD.Studio.PARAMETER_ID m_waterCaveDropID;
    private FMOD.Studio.PLAYBACK_STATE m_waterCaveDropPlaybackState;
    #endregion
    #endregion

    private void Start() {

        m_windPlainsInstance = FMODUnity.RuntimeManager.CreateInstance(m_windPlainsEvent);
        m_waterCaveDropInstance = FMODUnity.RuntimeManager.CreateInstance(m_waterCaveDropEvent);

        // Wind Plains parameter
        FMOD.Studio.EventDescription windPlainsEventDesc;
        m_windPlainsInstance.getDescription(out windPlainsEventDesc);
        FMOD.Studio.PARAMETER_DESCRIPTION windPlainsParametterDesc;
        windPlainsEventDesc.getParameterDescriptionByIndex(0, out windPlainsParametterDesc);
        m_windPlainsID = windPlainsParametterDesc.id;

        // Water Cave Drop parameter
        FMOD.Studio.EventDescription waterCaveDropEventDesc;
        m_waterCaveDropInstance.getDescription(out waterCaveDropEventDesc);
        FMOD.Studio.PARAMETER_DESCRIPTION waterCaveDropParametterDesc;
        waterCaveDropEventDesc.getParameterDescriptionByIndex(0, out waterCaveDropParametterDesc);
        m_waterCaveDropID = waterCaveDropParametterDesc.id;
    }

    private void Update() {
        m_windPlainsInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
        m_waterCaveDropInstance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform));
    }

    //private void OnTriggerEnter(Collider other) {
    //    if (other.CompareTag(m_windPlainsTag)) {
    //        StartWind();
    //    } else if (other.CompareTag(m_waterCaveDropTag)) {
    //        StartWater();
    //    }
    //}

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(m_windPlainsTag)) {
            StopWind();
        } else if (other.CompareTag(m_waterCaveDropTag)) {
            StopWater();
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag(m_windPlainsTag)) {
            m_windPlainsInstance.getPlaybackState(out m_windPlainsPlaybackState);
            if (m_windPlainsPlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
                m_windPlainsInstance.start();
            }
        } else if (other.CompareTag(m_waterCaveDropTag)) {
            m_waterCaveDropInstance.getPlaybackState(out m_waterCaveDropPlaybackState);
            if (m_waterCaveDropPlaybackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
                m_waterCaveDropInstance.start();
            }
        }
    }

    //private void StartWind() {
    //    FMOD.Studio.PLAYBACK_STATE playbackState;
    //    m_waterCaveDropInstance.getPlaybackState(out playbackState);
    //    if (playbackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
    //        m_windPlainsInstance.setParameterByID(m_waterCaveDropID, 0.0f);
    //        m_windPlainsInstance.start();
    //    }
    //}

    private void StopWind() {
        m_windPlainsInstance.setParameterByID(m_windPlainsID, 1.0f);
    }


    //private void StartWater() {
    //    FMOD.Studio.PLAYBACK_STATE playbackState;
    //    m_waterCaveDropInstance.getPlaybackState(out playbackState);
    //    if (playbackState != FMOD.Studio.PLAYBACK_STATE.PLAYING) {
    //        m_waterCaveDropInstance.setParameterByID(m_waterCaveDropID, 0.0f);
    //        m_waterCaveDropInstance.start();
    //    }
    //}

    private void StopWater() {
        m_waterCaveDropInstance.setParameterByID(m_waterCaveDropID, 1.0f);
    }

}
