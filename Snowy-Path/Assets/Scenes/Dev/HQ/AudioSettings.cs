using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour
{

    FMOD.Studio.EventInstance SFXVolumeTestEvent;

    static FMOD.Studio.Bus Music;
    static FMOD.Studio.Bus SFX;
    static FMOD.Studio.Bus Master;
    static FMOD.Studio.Bus ReverbCavern;
    public float MusicVolume;
    public float SFXVolume;
    public float MasterVolume;

    void Awake()
    {
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Music");
        SFX = FMODUnity.RuntimeManager.GetBus("bus:/SFX");
        Master = FMODUnity.RuntimeManager.GetBus("bus:/");
        ReverbCavern = FMODUnity.RuntimeManager.GetBus("bus:/ReverbCavern");
        SetVolume();
        //SFXVolumeTestEvent = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/SFXVolumeTest");
    }

    public void SetVolume()
    {
        Music.setVolume(MusicVolume);
        SFX.setVolume(SFXVolume);
        Master.setVolume(MasterVolume);
        ReverbCavern.setVolume(0f);
    }


}