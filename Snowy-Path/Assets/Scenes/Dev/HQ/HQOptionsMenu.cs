using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class HQOptionsMenu : MonoBehaviour
{
    //public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public TMPro.TMP_Dropdown aaDropdown;
    public Slider generalSlider;
    public Slider musicSlider;
    public Slider soundsSlider;
    public Slider gammaSlider;
    public AudioSettings audioSettings;
    private float GammaCorrection;
    Resolution[] resolutions;

    List<string> stringList = new List<string>();
    List<string[]> parsedList = new List<string[]>();

    private void Awake()
    {
        string serializedsettings =
        "Resolution, 1" + "\n" +
        "Antialiasing, 1" + "\n" +
        "Master Volume, 1" + "\n" +
        "Music Volume, 1" + "\n" +
        "Sounds Effect Volume, 1" + "\n" +
        "Gamma, 0.5" + "\n";

        string destination = Application.persistentDataPath + "/savesettings.txt";
        File.Create(destination).Dispose();
        File.WriteAllText(destination, serializedsettings);

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
        LoadSettings();
    }

    private void Start()
    {


    }

    private void OnEnable()
    {
        stringList = new List<string>();
        parsedList = new List<string[]>();
        LoadSettings();
    }


    public void SetResolution(int resolutionIndex)
    {
        resolutionDropdown.value = resolutionIndex;
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



    void Update()
    {

        RenderSettings.ambientLight = new Color(GammaCorrection, GammaCorrection, GammaCorrection, 1.0f);

    }

    public void SetGamma(float gammavalue)
    {

        GammaCorrection = gammavalue;

    }




    public void LoadSettings()
    {

        string destination = Application.persistentDataPath + "/savesettings.txt";
        if (File.Exists(destination))
        {
            readTextFiled(destination);
            parseList();
            //Debug.Log(parsedList[0][1]);
            //Debug.Log();
            SetResolution(int.Parse(parsedList[0][1]));
            SetQuality(int.Parse(parsedList[1][1]));
            audioSettings.MasterVolumeLevel(float.Parse(parsedList[2][1]));
            audioSettings.MusicVolumeLevel(float.Parse(parsedList[3][1]));
            audioSettings.SFXVolumeLevel(float.Parse(parsedList[4][1]));
            generalSlider.value = float.Parse(parsedList[2][1]);
            musicSlider.value = float.Parse(parsedList[3][1]);
            soundsSlider.value = float.Parse(parsedList[4][1]);
            gammaSlider.value = float.Parse(parsedList[5][1]);
        }

        else
        {
            Debug.LogError("File not found");
            return;
        }



    }

    public void SaveSettings()
    {
        string serializedsettings =
        "Resolution, " + resolutionDropdown.value.ToString() + "\n" +
        "Antialiasing, " + aaDropdown.value.ToString() + "\n" +
        "Master Volume, " + audioSettings.MasterVolume.ToString() + "\n" +
        "Music Volume, " + audioSettings.MusicVolume.ToString() + "\n" +
        "Sounds Effect Volume, " + audioSettings.SFXVolume.ToString() + "\n" +
        "Gamma, " + gammaSlider.value.ToString() + "\n";

        string destination = Application.persistentDataPath + "/savesettings.txt";

        if (File.Exists(destination))
            File.WriteAllText(destination, serializedsettings);
        else
            File.Create(destination).Dispose();
        File.WriteAllText(destination, serializedsettings);



    }



    void readTextFiled(string path)
    {
        StreamReader inp_stm = new StreamReader(path);

        while (!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();

            stringList.Add(inp_ln);
        }

        inp_stm.Close();


    }

    void parseList()
    {
        for (int i = 0; i < stringList.Count; i++)
        {
            string[] temp = stringList[i].Split(","[0]);
            for (int j = 0; j < temp.Length; j++)
            {
                temp[j] = temp[j].Trim();  //removed the blank spaces
            }
            parsedList.Add(temp);


        }

    }
}






