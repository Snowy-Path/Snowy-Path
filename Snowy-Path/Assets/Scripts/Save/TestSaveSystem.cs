using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestSaveSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null) {
            Debug.LogError("No keyboard connected");
            return;
        }

        if (keyboard.eKey.wasPressedThisFrame) {
            SaveSystem.Instance.FindAndLoadExistingSave();
        }

        if (keyboard.fKey.wasPressedThisFrame) {
            Debug.Log("input");
            SaveSystem.Instance.CreateNewSave(3);
        }

    }
}
