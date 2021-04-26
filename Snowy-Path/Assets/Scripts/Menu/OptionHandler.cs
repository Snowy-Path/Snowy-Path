using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionHandler : MonoBehaviour
{
    Resolution[] resolutions;
    public OptionSettings optionSettings;

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
            optionSettings = new OptionSettings
            {
                MasterVolume = 1f,
                MusicVolume = 0.5f,
                SFXVolume = 0.5f,
                resolution_index = currentResolutionIndex,
                aa_index = 0,
                gammavalue = 1
            };
            OptionSave.Save(optionSettings);
        }
        else
        {

            optionSettings = OptionSave.Load();
        }

    }


}

// Update is called once per frame

