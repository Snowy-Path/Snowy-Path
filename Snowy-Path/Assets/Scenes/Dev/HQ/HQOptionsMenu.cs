
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class HQOptionsMenu : MonoBehaviour
{
    //public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public TMPro.TMP_Dropdown aaDropdown;
    public Slider generalSlider;
    public Slider musicSlider;
    public Slider soundsSlider;
    public Slider gammaSlider;
    OptionHandler optionhandler;
    private float GammaCorrection;
    public Resolution[] resolutions;
    public List<int> AntiAliasing = new List<int>{0,2,4,8};


    private void Awake()
    {
        optionhandler = GameObject.FindObjectOfType<OptionHandler>();//("OptionSettings"); GameObject.F
        Debug.Log(optionhandler.name);
        Debug.Log(optionhandler.optionSettings);
        StartResolution();
        LoadSettings(optionhandler.optionSettings);
    }

    //        RenderSettings.ambientLight = new Color(GammaCorrection, GammaCorrection, GammaCorrection, 1.0f);






    public void SetResolution(int resolutionIndex)
    {
        resolutionDropdown.value = resolutionIndex;
        //LoadSettings(currentResolutionIndex);
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,
                  resolution.height, Screen.fullScreen);
        //optionhandler.optionSettings.resolution_index = resolutionIndex;


    }

    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = AntiAliasing[aaIndex];
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



    public void SetGamma(float gammavalue)
    {
        float newgamma = gammavalue;
        Screen.brightness = newgamma;

    }

    public int StartResolution()
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
        return currentResolutionIndex;
    }
    
    public void ApplySettings()
    {
        OptionSettings newsettings = new OptionSettings {
            MasterVolume = generalSlider.value,
            MusicVolume = musicSlider.value,
            SFXVolume = soundsSlider.value,
            aa_index = aaDropdown.value,
            resolution_index = resolutionDropdown.value,
            gammavalue = gammaSlider.value };
        
        OptionSave.Save(newsettings);
        optionhandler.optionSettings = newsettings;
        

    }

    public void BackSettings()
    {
        LoadSettings(optionhandler.optionSettings);
    }

    public void LoadSettings(OptionSettings settings)
    {
        generalSlider.value = settings.MasterVolume;
        musicSlider.value = settings.MusicVolume;
        soundsSlider.value = settings.SFXVolume;
        aaDropdown.value = settings.aa_index;
        resolutionDropdown.value = settings.resolution_index;
        gammaSlider.value = settings.gammavalue;
    }

}






