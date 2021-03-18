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
        LoadSettings(currentResolutionIndex);
    }

    public void SetVolume(float volume)
    {
        //audioMixer.SetFloat("Volume", volume);
        currentVolume = volume;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,
                  resolution.height, Screen.fullScreen);
    }

    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;

    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionPreference",
                   resolutionDropdown.value);
        PlayerPrefs.SetInt("AntiAliasingPreference",
                   aaDropdown.value);
        PlayerPrefs.SetFloat("VolumePreference",
                   currentVolume);
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



    void OnGUI()
    {
        /*keyEvent dictates what key our user presses
		 * bt using Event.current to detect the current
		 * event
		 */
        keyEvent = Event.current;

        //Executes if a button gets pressed and
        //the user presses a key
        if (keyEvent.isKey && waitingForKey)
        {
            newKey = keyEvent.keyCode; //Assigns newKey to the key user presses
            waitingForKey = false;
        }
    }

    /*Buttons cannot call on Coroutines via OnClick().
	 * Instead, we have it call StartAssignment, which will
	 * call a coroutine in this script instead, only if we
	 * are not already waiting for a key to be pressed.
	 */
    public void StartAssignment(string keyName)
    {
        if (!waitingForKey)
            StartCoroutine(AssignKey(keyName));
    }


    IEnumerator WaitForKey()
    {
        while (!keyEvent.isKey)
            yield return null;
    }


    public IEnumerator AssignKey(string keyName)
    {
        waitingForKey = true;

        yield return WaitForKey(); //Executes endlessly until user presses a key

        switch (keyName)
        {
            case "forward":

                break;
            case "backward":

                break;
            case "left":

                break;
            case "right":

                break;
            case "jump":

                break;
        }

        yield return null;
    }
}





