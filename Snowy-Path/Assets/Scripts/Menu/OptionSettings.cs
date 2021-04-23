using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.InputSystem;
using System;

[Serializable]
public class OptionSettings
{
    public float MasterVolume;
    public float MusicVolume;
    public float SFXVolume;
    public int resolution_index;
    public int aa_index;
    public float gammavalue;







    public static OptionSettings Load(string destination)
    {

        return JsonUtility.FromJson<OptionSettings>(destination);


    }

    public static string Save(OptionSettings optionsettings)
    {
        return JsonUtility.ToJson(optionsettings);


    }





}