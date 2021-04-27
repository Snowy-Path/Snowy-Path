using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionHandler : MonoBehaviour
{
    Resolution[] resolutions;
    public OptionSettings optionSettings;

    public Gamepad current { get; }
    public static bool gamepadconnected;
    // Start is called before the first frame update
    void Awake()
    {
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
        // If no settings save already, loading default settings
        if (!OptionSave.Init())
        {
            optionSettings = new OptionSettings
            {
                MasterVolume = 1f,
                MusicVolume = 0.5f,
                SFXVolume = 0.5f,
                resolution_index = currentResolutionIndex,
                aa_index = 2,
                gammavalue = 1
            };
            OptionSave.Save(optionSettings);
        }
        else
        {

            optionSettings = OptionSave.Load();
        }

        if (current != null)
        {
            gamepadconnected = true;
        }
        else
            gamepadconnected = false;


    }



}

// Update is called once per frame

