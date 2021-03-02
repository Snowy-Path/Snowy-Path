using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInteractable : MonoBehaviour
{
    public void LaunchSave() {
        SaveSystem.Instance.Save();
    }
}
