using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MapEventHandler : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    private Map m_map;
    private MapUI m_mapUI;
    private MapPinManager m_mapPinManager;

    private bool m_isDragging = false;

    void Start()
    {
        m_map = GetComponent<Map>();
        m_mapUI = GetComponentInParent<MapUI>();
        m_mapPinManager = GetComponent<MapPinManager>();

        m_mapUI.inputActionAsset.FindAction("Map/Confirm").performed += OnConfirm;
        m_mapUI.inputActionAsset.FindAction("Map/Cancel").performed += OnCancel;
        m_mapUI.inputActionAsset.FindAction("Map/Remove pin").performed += (InputAction.CallbackContext c) => m_mapPinManager.PinRemove();
        m_mapUI.inputActionAsset.FindAction("Map/Navigate").performed += OnNavigate;
        m_mapUI.inputActionAsset.FindAction("Map/Navigate").canceled += OnNavigate;
        m_mapUI.inputActionAsset.FindAction("Map/Zoom").performed += OnZoom;
        m_mapUI.inputActionAsset.FindAction("Map/Zoom").canceled += OnZoom;
    }

    //=========================================================================
    // Unity EventSystem callbacks
    //=========================================================================

    public void OnPointerClick(PointerEventData data)
    {
        if (!m_isDragging)
            m_mapPinManager.PlacePin(data.position);
    }

    public void OnBeginDrag(PointerEventData data)
    {
        m_isDragging = true;
        m_map.DragMap(data.delta);
    }

    public void OnDrag(PointerEventData data)
    {
        m_map.DragMap(data.delta);
    }

    public void OnEndDrag(PointerEventData data)
    {
        m_map.DragMap(data.delta);
        m_isDragging = false;
    }

    public void OnScroll(PointerEventData data)
    {
        m_map.Zoom(data.scrollDelta.y, data.position);
    }

    //=========================================================================
    // Unity InputSystem callbacks
    //=========================================================================

    void OnCancel(InputAction.CallbackContext context)
    {
        if (m_mapUI.IsInEditMode)
            m_mapPinManager.PinCancel(); // Close pin panel
        else
            m_mapUI.CloseFullscreenMap();
    }

    void OnConfirm(InputAction.CallbackContext context)
    {
        if (m_mapUI.IsInEditMode) { 
            m_mapPinManager.PinConfirm();
        }
        else {
            // To be able to select map pins with the controller, we need to simulate a click event
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = RectTransformUtility.WorldToScreenPoint(Camera.main, m_mapUI.MapCursor.transform.position);

            // Then, we send the event to the pins themselves
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (RaycastResult result in results) {
                if (result.gameObject.name == "FullMap" || result.gameObject.name == "MapPin") {
                    if (result.gameObject.name == "FullMap")
                        m_mapUI.PinPanel.SelectPinType(0);
                    else
                        m_mapUI.PinPanel.SelectPinType(result.gameObject.GetComponent<MapPin>().pinType);

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
        if (!m_mapUI.IsInEditMode) {
            m_mapUI.MapCursor.Velocity = value * m_mapUI.cursorSpeed;
        }
        // If pin panel is enabled, stop moving the cursor and change current pin type
        else {
            m_mapUI.MapCursor.Velocity = Vector2.zero;

            if (context.phase == InputActionPhase.Performed && m_canMovePinCursor) {
                m_mapUI.PinPanel.SelectNextPinType(new Vector2Int(
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
        m_map.ZoomVelocity = delta;
    }
}
