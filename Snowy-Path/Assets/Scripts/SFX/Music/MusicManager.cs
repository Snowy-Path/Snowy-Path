using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public static MusicManager Instance { get; private set; }

    [SerializeField]
    private bool m_playOnStart = false;

    [FMODUnity.EventRef]
    public string m_emitterDropEvent = "";
    private FMOD.Studio.EventInstance m_emitterInstance;


    private void Awake() {
        Instance = this;
    }

    private void Start() {
        if (m_playOnStart) {
            PlayMusic();
        }
    }

    public void ChangeParametter(string parametterName, float value) {
        m_emitterInstance.setParameterByName(parametterName, value);
    }

    internal void PlayMusic() {
        m_emitterInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        m_emitterInstance = FMODUnity.RuntimeManager.CreateInstance(m_emitterDropEvent);
        m_emitterInstance.start();
    }

    private void OnDestroy() {
        m_emitterInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
