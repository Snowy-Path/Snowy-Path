using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapTool : MonoBehaviour
{
    public GameObject mapObject;
    public Camera inGameMapCamera;

    private Map m_map;
    private Canvas m_mapCanvasComponent;
    private RectTransform m_mapRectTransform;
    private PlayerInput m_playerInput;
    private CanvasRenderer m_canvasRenderer;
    private bool m_isFullscreenMode = false;

    void Start()
    {
        m_map = mapObject.GetComponentInChildren<Map>();
        m_mapRectTransform = m_map.GetComponent<RectTransform>();
        m_playerInput = GetComponentInParent<PlayerInput>();
        m_mapCanvasComponent = mapObject.GetComponentInParent<Canvas>();
    }

    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard.mKey.wasPressedThisFrame) {
            if (!m_isFullscreenMode) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                m_playerInput.SwitchCurrentActionMap("UI");
                m_mapCanvasComponent.worldCamera = null;
                m_isFullscreenMode = true;
            }
            else {
                m_map.ClosePinPanel();

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                m_playerInput.SwitchCurrentActionMap("Gameplay");
                m_mapCanvasComponent.worldCamera = inGameMapCamera;
                m_isFullscreenMode = false;
            }
        }
    }
}
