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

            if (mapUI.activeSelf) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                GetComponentInParent<PlayerInput>().SwitchCurrentActionMap("UI");
            }
            else {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                GetComponentInParent<PlayerInput>().SwitchCurrentActionMap("Gameplay");
            }
        }
    }
}
