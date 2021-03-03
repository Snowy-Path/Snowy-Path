using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapCompass : MonoBehaviour, IHandTool {
    public EToolType ToolType { get => EToolType.MapCompass; }

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

    public void StartPrimaryUse() {
        Locate();
    } 
    
    public void CancelPrimaryUse() {
        Debug.Log("Stop using map and compass");
    }

    public void SecondaryUse() {
        if (!m_isFullscreenMode) {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            m_playerInput.SwitchCurrentActionMap("Map");
            m_mapCanvasComponent.worldCamera = null;
            m_isFullscreenMode = true;
        }
    }

    public void CloseFullscreenMap() {
        if (m_isFullscreenMode) {
            m_map.ClosePinPanel();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            m_playerInput.SwitchCurrentActionMap("Gameplay");
            m_mapCanvasComponent.worldCamera = inGameMapCamera;
            m_isFullscreenMode = false;
        }
    }

    public void ToggleDisplay(bool display) {
        gameObject.SetActive(display);
    }

    private void Locate() {
        Debug.Log("Youre here !");
    }
}
