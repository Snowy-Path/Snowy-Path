using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.IO;

public class HQOptionsMenu : MonoBehaviour
{
    //public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public TMPro.TMP_Dropdown aaDropdown;
    public Slider generalSlider;
    public Slider musicSlider;
    public Slider soundsSlider;
    public AudioSettings audioSettings;
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

    }


    public void SetResolution(int resolutionIndex)
    {

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



    public void LoadSettings(int currentResolutionIndex)
    {



    }

    public void SaveSettings()
    {
        string serializedsettings =
        "Resolution, " + resolutionDropdown.value.ToString() + "\n" +
        "Antialiasing, " + aaDropdown.value.ToString() + "\n" +
        "Master Volume, " + audioSettings.MasterVolume.ToString() + "\n" +
        "Music Volume, " + audioSettings.MusicVolume.ToString() + "\n" +
        "Sounds Effect Volume, " + audioSettings.SFXVolume.ToString() + "\n";

        string destination = Application.persistentDataPath + "/savesettings.txt";

        if (File.Exists(destination))
            File.WriteAllText(destination, serializedsettings);
        else
            File.Create(destination).Dispose();
            File.WriteAllText(destination, serializedsettings);


        // Read
        //StreamReader reader = new StreamReader("savesettings.txt");
        //string lineA = reader.ReadLine();
        //string[] splitA = lineA.Split(',');
        //scoreA = int.Parse(splitA[1]);

        //string lineB = reader.ReadLine();
        //string[] splitB = lineB.Split(',');
        //scoreB = int.Parse(splitB[1]);



    }

    public void LoadFile()
    {
        string destination = Application.persistentDataPath + "/savesettings.txt";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.LogError("File not found");
            return;
        }

        //BinaryFormatter bf = new BinaryFormatter();
        //GameData data = (GameData)bf.Deserialize(file);
        //file.Close();

        //currentScore = data.score;
        //currentName = data.name;
        //currentTimePlayed = data.timePlayed;

        //Debug.Log(data.name);
        //Debug.Log(data.score);
        //Debug.Log(data.timePlayed);
    }

}






