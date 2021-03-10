using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Map : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    public float mapPinScale = 1f;
    public float zoomStep = 0.1f;
    public float zoomMin = 1f;
    public float zoomMax = 5f;
    public float mouseDragSpeed = 100f;
    public MapPinPanel pinPanel;
    public MapCompass mapCompass;
    public InputActionAsset inputActionAsset;
    public RectTransform cursor;
    public float cursorSpeed = 200f;
    public float cursorScrollSpeed = 2f;
    public float controllerZoomSpeed = 0.5f;

    private bool m_isDragging = false;
    private bool m_isPinModeEnabled = false;
    private bool m_isEditingPin = false;
    private RectTransform m_rectTransform;
    private GameObject m_lastPinPlaced;
    private Vector3 m_lastPinPosition;
    private Color m_lastPinColor;
    private Vector2 m_cursorVelocity;
    private float m_zoomVelocity = 0f;

    void Start()
    {
        inputActionAsset.FindAction("Map/Confirm").performed += OnConfirm;
        inputActionAsset.FindAction("Map/Cancel").performed += OnCancel;
        inputActionAsset.FindAction("Map/Remove pin").performed += (InputAction.CallbackContext c) => PinRemove();
        inputActionAsset.FindAction("Map/Navigate").performed += OnNavigate;
        inputActionAsset.FindAction("Map/Navigate").canceled += OnNavigate;
        inputActionAsset.FindAction("Map/Zoom").performed += OnZoom;
        inputActionAsset.FindAction("Map/Zoom").canceled += OnZoom;

        if (pinPanel == null)
            Debug.LogError("[MapUIController] Can't find map pin panel.");
        else
            pinPanel.gameObject.SetActive(false);

        m_rectTransform = GetComponent<RectTransform>();
    }

    // Note: Here we apply the movement/zoom using the velocities (Gamepad mode only)
    void Update()
    {
        cursor.anchoredPosition = cursor.anchoredPosition + m_cursorVelocity * Time.deltaTime;
        m_rectTransform.anchoredPosition = Vector2.Lerp(m_rectTransform.anchoredPosition, -cursor.anchoredPosition * m_rectTransform.localScale, Time.deltaTime * cursorScrollSpeed);
        if (m_zoomVelocity != 0)
            Zoom(m_zoomVelocity, RectTransformUtility.WorldToScreenPoint(Camera.current, cursor.transform.position), true);
        else
            KeepMapCenteredInView();
    }

    public void ToggleCursorVisibility()
    {
        cursor.gameObject.SetActive(!cursor.gameObject.activeSelf);
    }

    //=========================================================================
    // Unity EventSystem callbacks
    //=========================================================================

    public void OnPointerClick(PointerEventData data)
    {
        if (!m_isDragging)
            PlacePin(data.position);
    }

    public void OnBeginDrag(PointerEventData data)
    {
        m_isDragging = true;
        DragMap(data);
    }

    public void OnDrag(PointerEventData data)
    {
        DragMap(data);
    }

    public void OnEndDrag(PointerEventData data)
    {
        DragMap(data);
        m_isDragging = false;
    }

    public void OnScroll(PointerEventData data)
    {
        Zoom(data.scrollDelta.y, data.position);
    }

    //=========================================================================
    // Unity InputSystem callbacks
    //=========================================================================

    void OnCancel(InputAction.CallbackContext context)
    {
        if (m_isPinModeEnabled)
            ClosePinPanel();
        else
            mapCompass.CloseFullscreenMap();
    }

    void OnConfirm(InputAction.CallbackContext context)
    {
        if (m_isPinModeEnabled) { 
            PinConfirm();
        }
        else {
            // To be able to select map pins with the controller, we need to simulate a click event
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = RectTransformUtility.WorldToScreenPoint(Camera.main, cursor.transform.position);

            // Then, we send the event to the pins themselves
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (RaycastResult result in results) {
                if (result.gameObject.name == "FullMap" || result.gameObject.name == "MapPin") {
                    if (result.gameObject.name == "FullMap")
                        pinPanel.SelectPinType(0);
                    else
                        pinPanel.SelectPinType(result.gameObject.GetComponent<MapPin>().pinType);

                    ExecuteEvents.Execute(result.gameObject, eventData, ExecuteEvents.pointerClickHandler);

                    break;
                }
            }
        }
    }

    private bool m_canMovePinCursor = false;
    void OnNavigate(InputAction.CallbackContext context)
    {
        // If pin panel is disabled, move the cursor
        var value = context.ReadValue<Vector2>();
        if (!m_isPinModeEnabled) {
            m_cursorVelocity = value * cursorSpeed;
        }
        // If pin panel is enabled, stop moving the cursor and change current pin type
        else {
            m_cursorVelocity = Vector2.zero;

            if (context.phase == InputActionPhase.Performed && m_canMovePinCursor) {
                pinPanel.SelectNextPinType(new Vector2Int(
                    value.x > 0 ? 1 : value.x < 0 ? -1 : 0,
                    value.y > 0 ? 1 : value.y < 0 ? -1 : 0
                ));
            }
        }

        if (context.phase == InputActionPhase.Canceled) {
            m_canMovePinCursor = true;
        }
        else if (context.phase == InputActionPhase.Performed) {
            m_canMovePinCursor = false;
        }
    }

    void OnZoom(InputAction.CallbackContext context)
    {
        var delta = context.ReadValue<float>();
        m_zoomVelocity = delta;
    }

    //=========================================================================
    // Pin management
    //=========================================================================

    public void PinConfirm()
    {
        m_isEditingPin = false;
        m_isPinModeEnabled = false;
        pinPanel.gameObject.SetActive(false);
        m_lastPinPlaced = null;
    }

    public void PinRemove()
    {
        m_isEditingPin = false;
        m_isPinModeEnabled = false;
        pinPanel.gameObject.SetActive(false);
        Destroy(m_lastPinPlaced);
        m_lastPinPlaced = null;
    }

    public void PinCancel()
    {
        m_isPinModeEnabled = false;
        pinPanel.gameObject.SetActive(false);

        if (!m_isEditingPin) {
            Destroy(m_lastPinPlaced);
            m_lastPinPlaced = null;
        }
        else {
            m_lastPinPlaced.transform.position = m_lastPinPosition;
            m_lastPinPlaced.GetComponent<Image>().color = m_lastPinColor;
            m_lastPinPlaced = null;
        }

        m_isEditingPin = false;
    }

    void OnPinClick(GameObject pin)
    {
        m_isEditingPin = true;
        m_lastPinPosition = pin.transform.position;
        m_lastPinPlaced = pin;
        m_lastPinColor = pin.GetComponent<Image>().color;

        OpenPinPanel();
    }

    public void OnPinTypeChanged()
    {
        if (m_lastPinPlaced != null) {
            m_lastPinPlaced.GetComponent<Image>().color = pinPanel.CurrentPinColor;
            m_lastPinPlaced.GetComponent<MapPin>().pinType = pinPanel.CurrentPinType;
        }
    }

    public void OpenPinPanel()
    {
        m_isPinModeEnabled = true;
        pinPanel.gameObject.SetActive(true);
    }

    public void ClosePinPanel()
    {
        PinCancel();
    }

    void PlacePin(Vector2 position)
    {
        if (m_lastPinPlaced != null) {
            Destroy(m_lastPinPlaced);
            m_lastPinPlaced = null;
        }

        int currentPinType = pinPanel.CurrentPinType;
        if (!m_isPinModeEnabled) {
            m_isPinModeEnabled = true;
            pinPanel.gameObject.SetActive(true);
        }

        GameObject child = new GameObject("MapPin");
        child.transform.parent = transform;

        RectTransform rectTransform = child.AddComponent<RectTransform>();
        rectTransform.localScale = new Vector3(
            mapPinScale / transform.localScale.x,
            mapPinScale / transform.localScale.y,
            mapPinScale / transform.localScale.z
        );

        Vector2 relativePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, position, null, out relativePosition);

        rectTransform.anchoredPosition = relativePosition;

        Image image = child.AddComponent<Image>();
        image.color = pinPanel.CurrentPinColor;

        Button button = child.AddComponent<Button>();
        button.onClick.AddListener(() => OnPinClick(child));

        MapPin pin = child.AddComponent<MapPin>();
        pin.scale = mapPinScale;
        pin.pinType = pinPanel.CurrentPinType;

        m_lastPinPlaced = child;
    }

    //=========================================================================
    // Map movement and zoom
    //=========================================================================

    void Zoom(float scrollDelta, Vector2 position, bool isController = false)
    {
        float scroll = scrollDelta;
        float zoomStep = this.zoomStep;
        if (isController)
            zoomStep *= controllerZoomSpeed;

        Vector3 newScale = Vector3.zero;
        if (scroll > 0) {
            newScale = m_rectTransform.localScale + new Vector3(zoomStep, zoomStep, 0f);
        }
        else if (scroll < 0) {
            newScale = m_rectTransform.localScale - new Vector3(zoomStep, zoomStep, 0f);
        }

        if (scroll != 0 && newScale.x >= zoomMin && newScale.x <= zoomMax) {
            m_rectTransform.localScale = newScale;

            Vector2 mousePosition = position;
            Vector2 relativeMousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, mousePosition, null, out relativeMousePosition);
            if (scroll > 0)
                m_rectTransform.anchoredPosition -= relativeMousePosition * zoomStep;
            else if (scroll < 0)
                m_rectTransform.anchoredPosition += relativeMousePosition * zoomStep;

            // Debug.Log((scroll > 0 ? "+" : "-") + relativeMousePosition * zoomStep + " | " + mousePosition + " | "  + m_rectTransform.position);
        }

        if (isController)
            m_rectTransform.anchoredPosition = -cursor.anchoredPosition * m_rectTransform.localScale;

        KeepMapCenteredInView();

        foreach (MapPin mapPin in GetComponentsInChildren<MapPin>())
            mapPin.UpdateScale();

        cursor.transform.localScale = new Vector3(
            1f / cursor.transform.parent.localScale.x,
            1f / cursor.transform.parent.localScale.y,
            1f / cursor.transform.parent.localScale.z
        );
    }

    void DragMap(PointerEventData data)
    {
        var delta = data.delta * mouseDragSpeed;
        m_rectTransform.anchoredPosition = Vector2.Lerp(m_rectTransform.anchoredPosition, m_rectTransform.anchoredPosition + delta, Time.deltaTime);

        KeepMapCenteredInView();
    }

    Vector2 GetMapViewBounds()
    {
        return new Vector2(
            (m_rectTransform.localScale.x - 1f) * (m_rectTransform.rect.width / 2f),
            (m_rectTransform.localScale.y - 1f) * (m_rectTransform.rect.height / 2f)
        );
    }

    void KeepMapCenteredInView()
    {
        var viewBounds = GetMapViewBounds();
        m_rectTransform.anchoredPosition = new Vector2(
            Mathf.Clamp(m_rectTransform.anchoredPosition.x, -viewBounds.x, viewBounds.x),
            Mathf.Clamp(m_rectTransform.anchoredPosition.y, -viewBounds.y, viewBounds.y)
        );
    }
}
