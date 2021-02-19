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

    void Start()
    {
    }

    public void Zoom(InputAction.CallbackContext context)
    {
        float scroll = context.ReadValue<float>();
        Debug.Log(Time.time + "-" + scroll);

        Vector3 newScale = Vector3.zero;
        if (scroll > 0) {
            newScale = map.localScale + new Vector3(zoomStep, zoomStep, zoomStep);
        }
        else if (scroll < 0) {
            newScale = map.localScale - new Vector3(zoomStep, zoomStep, zoomStep);
        }

        if (scroll != 0 && newScale.x >= zoomMin && newScale.x <= zoomMax)
            map.localScale = newScale;
    }
}
