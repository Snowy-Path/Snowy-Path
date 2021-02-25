using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestReloadCampfire : MonoBehaviour
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

        if (keyboard.f1Key.wasPressedThisFrame) {
            SaveSystem.Instance.Load();
        }
    }
}
