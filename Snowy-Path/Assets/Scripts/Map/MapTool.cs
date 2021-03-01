using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapTool : MonoBehaviour
{
    public GameObject mapObject;
    public RectTransform mapPreview;

    private RectTransform m_map;
    private PlayerInput m_playerInput;
    private CanvasRenderer m_canvasRenderer;

    void Start()
    {
        m_map = mapObject.GetComponentInChildren<Map>().GetComponent<RectTransform>();
        m_playerInput = GetComponentInParent<PlayerInput>();
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard.mKey.wasPressedThisFrame) {
            mapObject.SetActive(!mapObject.activeSelf);

            if (mapObject.activeSelf) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                m_playerInput.SwitchCurrentActionMap("UI");
            }
            else {
                mapPreview.localScale = m_map.localScale;
                mapPreview.anchoredPosition = m_map.anchoredPosition;

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                m_playerInput.SwitchCurrentActionMap("Gameplay");
            }
        }
    }
}
