using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Utils {
    public static T FindComponent<T>(string objectPath)
    {
        var obj = GameObject.Find(objectPath);
        if (obj == null)
            throw new System.Exception("Unable to find object named '" + objectPath + "' in the scene.");

        return obj.GetComponent<T>();
    }
}

public class MapUI : MonoBehaviour
{
    [Tooltip("Reference to the input action asset.")]
    public InputActionAsset inputActionAsset;

    [Tooltip("Size of a single pin. 1 means 100 pixels on the screen.")]
    public float mapPinScale = 1f;

    [Tooltip("Zoom increase on a single action (mouse only). See controllerZoomSpeed for controller mode.")]
    public float zoomStep = 0.1f;

    [Tooltip("Minimum zoom value.")]
    public float zoomMin = 1f;

    [Tooltip("Maximum zoom value.")]
    public float zoomMax = 5f;

    [Tooltip("Speed at which the mouse will drag the map.")]
    public float mouseDragSpeed = 100f;

    [Tooltip("Movement speed of the cursor")]
    public float cursorSpeed = 200f;

    [Tooltip("Speed at which the map will scroll when moving the cursor.")]
    public float cursorScrollSpeed = 5f;

    [Tooltip("Speed at which the map will be zoomed when using a controller. See zoomStep for mouse zoom.")]
    public float controllerZoomSpeed = 0.25f;

    public MapPinPanel PinPanel { get { return m_mapPinPanel; } }
    public MapCursor MapCursor { get { return m_mapCursor; } }

    private Map m_map;
    private MapCursor m_mapCursor;
    private MapPinPanel m_mapPinPanel;
    private MapPinManager m_mapPinManager;
    private Canvas m_mapCanvasComponent;
    private MapCompass m_mapCompassTool;

    private enum State {
        InGame,
        Fullscreen,
        Edit
    };

    private State m_state = State.InGame;

    public bool IsInEditMode { get { return m_state == State.Edit; } }

    private bool m_isControllerModeEnabled = true;
    public bool IsControllerModeEnabled { get { return m_isControllerModeEnabled; } set { m_isControllerModeEnabled = value; } }

    void Awake()
    {
        // Find all the needed objects
        m_map = GetComponentInChildren<Map>();
        m_mapCursor = GetComponentInChildren<MapCursor>();
        m_mapPinPanel = GetComponentInChildren<MapPinPanel>();
        m_mapPinManager = GetComponentInChildren<MapPinManager>();
        m_mapCanvasComponent = GetComponentInParent<Canvas>();
        m_mapCompassTool = Utils.FindComponent<MapCompass>("MapCompass");

        // Init and hide the cursor by default, it shouldn't be displayed on the in-game map anyways
        m_mapCursor.Init();
        m_mapCursor.gameObject.SetActive(false);

        // Init and hide the pin panel
        m_mapPinPanel.Init();
        m_mapPinPanel.gameObject.SetActive(false);
    }

    public void OpenFullscreenMap()
    {
        if (m_state == State.InGame) {
            // Unlock and show mouse
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // Change input action map and canvas camera
            m_mapCompassTool.SwitchCurrentActionMap("Map");
            m_mapCanvasComponent.worldCamera = null;

            m_state = State.Fullscreen;
        }
    }

    public void CloseFullscreenMap()
    {
        if (m_state != State.InGame) {
            // Cancel pin placement and close pin panel
            m_mapPinManager.PinCancel();

            // Hide cursor
            m_mapCursor.gameObject.SetActive(false);

            // Lock and hide mouse
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            m_mapCompassTool.SwitchCurrentActionMap("Gameplay");
            m_mapCanvasComponent.worldCamera = m_mapCompassTool.InGameMapCamera;

            m_state = State.InGame;
        }
    }

    public void OpenPinPanel()
    {
        if (m_state == State.Fullscreen) {
            m_state = State.Edit;
            m_mapPinPanel.OpenPanel();
        }
    }

    public void ClosePinPanel()
    {
        if (m_state == State.Edit) {
            m_state = State.Fullscreen;
            m_mapPinPanel.ClosePanel();
        }
    }
}
