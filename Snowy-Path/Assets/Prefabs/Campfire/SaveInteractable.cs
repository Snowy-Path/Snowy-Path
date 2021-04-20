using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SaveInteractable : MonoBehaviour
{
    public void LaunchSave() {
        if (SaveSystem.Instance)
            SaveSystem.Instance.Save();
        else
            Debug.LogError("Can't find SaveSystem in scene");
    }
}
