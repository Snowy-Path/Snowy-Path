using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapTool : MonoBehaviour
{
    public GameObject map;

    private PlayerInput m_playerInput;

    void Start()
    {
        m_playerInput = GetComponentInParent<PlayerInput>();
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard.mKey.wasPressedThisFrame) {
            map.SetActive(!map.activeSelf);

            if (map.activeSelf) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                m_playerInput.SwitchCurrentActionMap("UI");
            }
            else {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                m_playerInput.SwitchCurrentActionMap("Gameplay");
            }
        }
    }
}
