using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;


public class OptionSettingsPauseMenu : MonoBehaviour
{
    public AudioSettings audioSettings;

    public HQOptionsMenu OptionMenu;
    public static Gamepad current { get; }
    public bool gamepadconnected = false;

    private void Awake()
    {
        string destination = Application.persistentDataPath + "/savesettings.txt";

        List<string[]> parsedList = parseList(readTextFiled(destination));
        //OptionMenu.resolutionDropdown.value = int.Parse(parsedList[0][1]);
        OptionMenu.aaDropdown.value =int.Parse(parsedList[1][1]);
        OptionMenu.generalSlider.value = float.Parse(parsedList[2][1]);
        OptionMenu.musicSlider.value = float.Parse(parsedList[3][1]);
        OptionMenu.soundsSlider.value = float.Parse(parsedList[4][1]);
        OptionMenu.gammaSlider.value = float.Parse(parsedList[5][1]);
    }

    private void Update()
    {
        if (current != null)
        {
            gamepadconnected = true;
        }
    }

    public void LoadDefaultSettings(int currentResolutionIndex)
    {
        string serializedsettings =
        "Resolution, " + currentResolutionIndex.ToString() + "\n" +
        "Antialiasing, 1" + "\n" +
        "Master Volume, 1" + "\n" +
        "Music Volume, 0.5" + "\n" +
        "Sounds Effect Volume, 0.5" + "\n" +
        "Gamma, 0.5" + "\n";

        string destination = Application.persistentDataPath + "/savesettings.txt";
        File.Create(destination).Dispose();
        File.WriteAllText(destination, serializedsettings);
        List<string[]> parsedList = parseList(readTextFiled(destination));
        OptionMenu.SetResolution(int.Parse(parsedList[0][1]));
        OptionMenu.SetQuality(int.Parse(parsedList[1][1]));
        audioSettings.MasterVolumeLevel(float.Parse(parsedList[2][1]));
        audioSettings.MusicVolumeLevel(float.Parse(parsedList[3][1]));
        audioSettings.SFXVolumeLevel(float.Parse(parsedList[4][1]));
        OptionMenu.generalSlider.value = float.Parse(parsedList[2][1]);
        OptionMenu.musicSlider.value = float.Parse(parsedList[3][1]);
        OptionMenu.soundsSlider.value = float.Parse(parsedList[4][1]);
        OptionMenu.gammaSlider.value = float.Parse(parsedList[5][1]);

    }

    public void LoadSettings()
    {


        string destination = Application.persistentDataPath + "/savesettings.txt";

        List<string[]> parsedList = parseList(readTextFiled(destination));
        OptionMenu.SetResolution(int.Parse(parsedList[0][1]));
        OptionMenu.SetQuality(int.Parse(parsedList[1][1]));
        audioSettings.MasterVolumeLevel(float.Parse(parsedList[2][1]));
        audioSettings.MusicVolumeLevel(float.Parse(parsedList[3][1]));
        audioSettings.SFXVolumeLevel(float.Parse(parsedList[4][1]));
        OptionMenu.generalSlider.value = float.Parse(parsedList[2][1]);
        OptionMenu.musicSlider.value = float.Parse(parsedList[3][1]);
        OptionMenu.soundsSlider.value = float.Parse(parsedList[4][1]);
        OptionMenu.gammaSlider.value = float.Parse(parsedList[5][1]);

    }

    public void SaveSettings()
    {
        string serializedsettings =
        "Resolution, " + OptionMenu.resolutionDropdown.value.ToString() + "\n" +
        "Antialiasing, " + OptionMenu.aaDropdown.value.ToString() + "\n" +
        "Master Volume, " + audioSettings.MasterVolume.ToString() + "\n" +
        "Music Volume, " + audioSettings.MusicVolume.ToString() + "\n" +
        "Sounds Effect Volume, " + audioSettings.SFXVolume.ToString() + "\n" +
        "Gamma, " + OptionMenu.gammaSlider.value.ToString() + "\n";

        string destination = Application.persistentDataPath + "/savesettings.txt";
        File.WriteAllText(destination, serializedsettings);



    }



    List<string> readTextFiled(string path)
    {
        List<string> stringList = new List<string>();
        StreamReader inp_stm = new StreamReader(path);

        while (!inp_stm.EndOfStream)
        {
            string inp_ln = inp_stm.ReadLine();

            stringList.Add(inp_ln);
        }

        inp_stm.Close();
        return stringList;

    }

    List<string[]> parseList(List<string> stringList)
    {
        List<string[]> parsedList = new List<string[]>();
        for (int i = 0; i < stringList.Count; i++)
        {
            string[] temp = stringList[i].Split(","[0]);
            for (int j = 0; j < temp.Length; j++)
            {
                temp[j] = temp[j].Trim();  //removed the blank spaces
            }
            parsedList.Add(temp);
        }
        return parsedList;

    }





}





