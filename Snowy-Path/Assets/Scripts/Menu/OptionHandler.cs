using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionHandler : MonoBehaviour
{
    public Resolution[] resolutions;

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

        if (!OptionSave.Init())
        {
            OptionSettings optionSettings = new OptionSettings
            {
                MasterVolume = 1f,
                MusicVolume = 0.5f,
                SFXVolume = 0.5f,
                resolution_index = currentResolutionIndex,
                aa_index = 0,
                gammavalue = 1
            };
            SaveSettings(optionSettings);
        }
        else
        {

            OptionSettings optionSettings = OptionSettings.Load(OptionSave.destination);
        }

    }


    public void SaveSettings(OptionSettings optionsettings)
    {
        string json= JsonUtility.ToJson(optionsettings);
        OptionSave.SaveSettings(json);
        Debug.Log(json);
    }

    public void LoadSettings()
    {
        OptionSettings optionSettings = OptionSettings.Load(OptionSave.destination);

    }
}

// Update is called once per frame

