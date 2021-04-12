using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class HQOptionsMenu : MonoBehaviour
{
    //public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public TMPro.TMP_Dropdown aaDropdown;
    public Slider volumeSlider;
    float currentVolume;
    Resolution[] resolutions;

    Transform menuPanel;
    Event keyEvent;
    Text buttonText;
    KeyCode newKey;


    bool waitingForKey;

    private void Start()
    {
        

    }

    public void SetVolume(float volume)
    {
        //audioMixer.SetFloat("Volume", volume);
        currentVolume = volume;
    }

    public void SetResolution(int resolutionIndex)
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " +
                     resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width
                  && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        //LoadSettings(currentResolutionIndex);
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,
                  resolution.height, Screen.fullScreen);
    }

    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;

    }


    public void SetQuality(int qualityIndex)
    {
        if (qualityIndex != 6) // if the user is not using 
                               //any of the presets
            QualitySettings.SetQualityLevel(qualityIndex);
        switch (qualityIndex)
        {
            case 0: // quality level - very low
                //textureDropdown.value = 3;
                aaDropdown.value = 0;
                break;
            case 1: // quality level - low
                //textureDropdown.value = 2;
                aaDropdown.value = 0;
                break;
            case 2: // quality level - medium
                //textureDropdown.value = 1;
                aaDropdown.value = 0;
                break;
            case 3: // quality level - high
                //textureDropdown.value = 0;
                aaDropdown.value = 0;
                break;
            case 4: // quality level - very high
               // textureDropdown.value = 0;
                aaDropdown.value = 1;
                break;
            case 5: // quality level - ultra
                //textureDropdown.value = 0;
                aaDropdown.value = 2;
                break;
        }

        //qualityDropdown.value = qualityIndex;
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionPreference",
                   resolutionDropdown.value);
        PlayerPrefs.SetInt("AntiAliasingPreference",
                   aaDropdown.value);
        PlayerPrefs.SetFloat("VolumePreference",
                   currentVolume);
        SetResolution(resolutionDropdown.value);
    }


    public void LoadSettings(int currentResolutionIndex)
    {

        if (PlayerPrefs.HasKey("ResolutionPreference"))
            resolutionDropdown.value =
                         PlayerPrefs.GetInt("ResolutionPreference");
        else
            resolutionDropdown.value = currentResolutionIndex;

        if (PlayerPrefs.HasKey("AntiAliasingPreference"))
            aaDropdown.value =
                         PlayerPrefs.GetInt("AntiAliasingPreference");
        else
            aaDropdown.value = 1;

        if (PlayerPrefs.HasKey("VolumePreference"))
            volumeSlider.value =
                        PlayerPrefs.GetFloat("VolumePreference");
        else
            volumeSlider.value =
                        PlayerPrefs.GetFloat("VolumePreference");
    }



}





