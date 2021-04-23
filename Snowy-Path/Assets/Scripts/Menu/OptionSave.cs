using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class OptionSave
{
    public static readonly string destination = Application.persistentDataPath + "/savesettings.json";
    public static bool Init()
    {

        if (File.Exists(destination))
        {
            return true;
        }

        else
        {
            return false;
        }
    }
 
    public static void SaveSettings(string serializedsettings)
    {

        File.WriteAllText(destination, serializedsettings);

    }

    public static void LoadSettings()
    {

    }

}
