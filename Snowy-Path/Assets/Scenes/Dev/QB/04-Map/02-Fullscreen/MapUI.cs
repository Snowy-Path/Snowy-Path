using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapUI : MonoBehaviour
{
    public RectTransform map;
    public float zoomStep = 0.1f;
    public float zoomMin = 1f;
    public float zoomMax = 5f;

    public float mouseDragSpeed = 100f;

    void Start()
    {
    }

    public void Zoom(InputAction.CallbackContext context)
    {
        float scroll = context.ReadValue<float>();

        Vector3 newScale = Vector3.zero;
        if (scroll > 0) {
            newScale = map.localScale + new Vector3(zoomStep, zoomStep, 0f);
        }
        else if (scroll < 0) {
            newScale = map.localScale - new Vector3(zoomStep, zoomStep, 0f);
        }

        if (scroll != 0 && newScale.x >= zoomMin && newScale.x <= zoomMax) {
            map.localScale = newScale;

            // IMPORTANT: Temporary code for Keyboard+Mouse controls only
            // Controllers will probably use a dedicated cursor
            var mouse = Mouse.current;
            if (mouse == null) {
                Debug.LogError("[MapUI] Can't find a mouse");
            }
            else {
                Vector2 mousePosition = mouse.position.ReadValue();
                Vector2 relativeMousePosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(map, mousePosition, null, out relativeMousePosition);
                if (scroll > 0)
                    map.anchoredPosition -= relativeMousePosition * zoomStep;
                else if (scroll < 0)
                    map.anchoredPosition += relativeMousePosition * zoomStep;

                Debug.Log((scroll > 0 ? "+" : "-") + relativeMousePosition * zoomStep + " | " + mousePosition + " | "  + map.position);
            }
        }

        KeepMapCenteredInView();
    }

    public void Move(InputAction.CallbackContext context)
    {
        var mouseDelta = context.ReadValue<Vector2>();
        var delta = mouseDelta * mouseDragSpeed;
        map.anchoredPosition = Vector2.Lerp(map.anchoredPosition, map.anchoredPosition + delta, Time.deltaTime);

        KeepMapCenteredInView();
    }

    Vector2 GetMapViewBounds()
    {
        return new Vector2(
            (map.localScale.x - 1f) * (map.rect.width / 2f),
            (map.localScale.y - 1f) * (map.rect.height / 2f)
        );
    }

    void KeepMapCenteredInView()
    {
        var viewBounds = GetMapViewBounds();
        map.anchoredPosition = new Vector2(
            Mathf.Clamp(map.anchoredPosition.x, -viewBounds.x, viewBounds.x),
            Mathf.Clamp(map.anchoredPosition.y, -viewBounds.y, viewBounds.y)
        );
    }
}
