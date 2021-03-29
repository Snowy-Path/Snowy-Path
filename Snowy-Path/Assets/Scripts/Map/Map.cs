using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    private MapUI m_mapUI;
    private RectTransform m_rectTransform;
    private float m_zoomVelocity = 0f;
    public float ZoomVelocity { set { m_zoomVelocity = value; } }

    void Start()
    {
        m_mapUI = GetComponentInParent<MapUI>();
        m_rectTransform = GetComponent<RectTransform>();
    }

    // Note: Here we apply the movement/zoom using the velocities (Gamepad mode only)
    void Update()
    {
        if (m_mapUI.IsControllerModeEnabled) {
            m_mapUI.MapCursor.UpdatePosition();

            m_rectTransform.anchoredPosition = Vector2.Lerp(m_rectTransform.anchoredPosition, -m_mapUI.MapCursor.AnchoredPosition * m_rectTransform.localScale, Time.deltaTime * m_mapUI.cursorScrollSpeed);

            if (m_zoomVelocity != 0)
                Zoom(m_zoomVelocity, RectTransformUtility.WorldToScreenPoint(Camera.current, m_mapUI.MapCursor.transform.position), true);
            else
                KeepMapCenteredInView();
        }
    }

    public void Zoom(float scrollDelta, Vector2 position, bool isController = false)
    {
        float scroll = scrollDelta;
        float zoomStep = m_mapUI.zoomStep;
        if (isController)
            zoomStep *= m_mapUI.controllerZoomSpeed;

        Vector3 newScale = Vector3.zero;
        if (scroll > 0) {
            newScale = m_rectTransform.localScale + new Vector3(zoomStep, zoomStep, 0f);
        }
        else if (scroll < 0) {
            newScale = m_rectTransform.localScale - new Vector3(zoomStep, zoomStep, 0f);
        }

        if (scroll != 0 && newScale.x >= m_mapUI.zoomMin && newScale.x <= m_mapUI.zoomMax) {
            m_rectTransform.localScale = newScale;

            Vector2 mousePosition = position;
            Vector2 relativeMousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, mousePosition, null, out relativeMousePosition);
            if (scroll > 0)
                m_rectTransform.anchoredPosition -= relativeMousePosition * zoomStep;
            else if (scroll < 0)
                m_rectTransform.anchoredPosition += relativeMousePosition * zoomStep;
        }

        if (isController)
            m_rectTransform.anchoredPosition = -m_mapUI.MapCursor.AnchoredPosition * m_rectTransform.localScale;

        KeepMapCenteredInView();

        foreach (MapPin mapPin in GetComponentsInChildren<MapPin>())
            mapPin.UpdateScale();

        m_mapUI.MapCursor.UpdateScale();
    }

    public void DragMap(Vector2 delta)
    {
        var movement = delta * m_mapUI.mouseDragSpeed;
        m_rectTransform.anchoredPosition = Vector2.Lerp(m_rectTransform.anchoredPosition, m_rectTransform.anchoredPosition + movement, Time.deltaTime);

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
