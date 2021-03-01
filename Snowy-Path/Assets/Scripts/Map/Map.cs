using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Map : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    public float mapPinScale = 1f;
    public float zoomStep = 0.1f;
    public float zoomMin = 1f;
    public float zoomMax = 5f;
    public float mouseDragSpeed = 100f;
    public MapPinPanel pinPanel;

    private bool m_isDragging = false;
    private bool m_isPinModeEnabled = false;
    private bool m_isEditingPin = false;
    private RectTransform m_rectTransform;
    private GameObject m_lastPinPlaced;

    void Start()
    {
        if (pinPanel == null)
            Debug.LogError("[MapUIController] Can't find map pin panel.");
        else
            pinPanel.gameObject.SetActive(false);

        m_rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData data)
    {
        if (!m_isDragging)
            PlacePin(data);
    }

    public void OnBeginDrag(PointerEventData data)
    {
        // Debug.Log("begin drag:" + data);
        m_isDragging = true;
        DragMap(data);
    }

    public void OnDrag(PointerEventData data)
    {
        // Debug.Log("drag:" + data);
        DragMap(data);
    }

    public void OnEndDrag(PointerEventData data)
    {
        // Debug.Log("end drag:" + data);
        DragMap(data);
        m_isDragging = false;
    }

    public void OnScroll(PointerEventData data)
    {
        Zoom(data);
    }

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

        m_isEditingPin = false;
    }

    void OnPinClick(GameObject pin)
    {
        m_isEditingPin = true;
        m_lastPinPlaced = pin;
        m_isPinModeEnabled = true;
        pinPanel.gameObject.SetActive(true);
    }

    public void OnPinTypeChanged()
    {
        if (m_lastPinPlaced != null)
            m_lastPinPlaced.GetComponent<Image>().color = pinPanel.CurrentPinColor;
    }

    void PlacePin(PointerEventData data)
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

        // FIXME: When m_isPinModeEnabled, and for all the duration of the mode, the same pin should be edited instead of creating new ones each time

        GameObject child = new GameObject("MapPin");
        child.transform.parent = transform;

        RectTransform rectTransform = child.AddComponent<RectTransform>();
        rectTransform.localScale = new Vector3(
            mapPinScale / transform.localScale.x,
            mapPinScale / transform.localScale.y,
            mapPinScale / transform.localScale.z
        );

        Vector2 mousePosition = data.position;
        Vector2 relativeMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, mousePosition, null, out relativeMousePosition);

        rectTransform.anchoredPosition = relativeMousePosition;

        Image image = child.AddComponent<Image>();
        image.color = pinPanel.CurrentPinColor;

        Button button = child.AddComponent<Button>();
        button.onClick.AddListener(() => OnPinClick(child));

        child.AddComponent<MapPin>().scale = mapPinScale;

        m_lastPinPlaced = child;
    }

    void Zoom(PointerEventData data)
    {
        float scroll = data.scrollDelta.y;

        Vector3 newScale = Vector3.zero;
        if (scroll > 0) {
            newScale = m_rectTransform.localScale + new Vector3(zoomStep, zoomStep, 0f);
        }
        else if (scroll < 0) {
            newScale = m_rectTransform.localScale - new Vector3(zoomStep, zoomStep, 0f);
        }

        if (scroll != 0 && newScale.x >= zoomMin && newScale.x <= zoomMax) {
            m_rectTransform.localScale = newScale;

            Vector2 mousePosition = data.position;
            Vector2 relativeMousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, mousePosition, null, out relativeMousePosition);
            if (scroll > 0)
                m_rectTransform.anchoredPosition -= relativeMousePosition * zoomStep;
            else if (scroll < 0)
                m_rectTransform.anchoredPosition += relativeMousePosition * zoomStep;

            // Debug.Log((scroll > 0 ? "+" : "-") + relativeMousePosition * zoomStep + " | " + mousePosition + " | "  + m_rectTransform.position);
        }

        KeepMapCenteredInView();

        foreach (MapPin mapPin in GetComponentsInChildren<MapPin>())
            mapPin.UpdateScale();
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
