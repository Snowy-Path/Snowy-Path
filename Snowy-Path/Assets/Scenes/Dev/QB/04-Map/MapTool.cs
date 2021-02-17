using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapTool : MonoBehaviour
{
    public GameObject mapUI;

    void Start()
    {
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard.mKey.wasPressedThisFrame) {
            mapUI.SetActive(!mapUI.activeSelf);
        }
    }
}
