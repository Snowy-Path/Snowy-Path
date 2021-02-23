using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MapUIController : MonoBehaviour
{
    public float mapPinScale = 1f;

    private MapUI m_mapUI;
    private MapPinPanel m_pinPanel;

    void Start()
    {
        m_mapUI = GetComponentInChildren<MapUI>();
        m_pinPanel = GetComponentInChildren<MapPinPanel>();
    }

    void Update()
    {
    }

    public void OnPinPlaced(InputAction.CallbackContext context)
    {
        if (context.canceled) {
            int currentPinType = m_pinPanel.CurrentPinType;
            if (currentPinType != -1) {
                GameObject child = new GameObject("MapPin");
                child.transform.parent = m_mapUI.map;

                RectTransform rectTransform = child.AddComponent<RectTransform>();
                rectTransform.localScale = new Vector3(
                    mapPinScale / m_mapUI.map.localScale.x,
                    mapPinScale / m_mapUI.map.localScale.y,
                    mapPinScale / m_mapUI.map.localScale.z
                );

                // IMPORTANT: Temporary code for Keyboard+Mouse controls only
                // Controllers will probably use a dedicated cursor
                var mouse = Mouse.current;
                if (mouse == null) {
                    Debug.LogError("[MapUI] Can't find a mouse");
                }
                else {
                    Vector2 mousePosition = mouse.position.ReadValue();
                    Vector2 relativeMousePosition;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(m_mapUI.map, mousePosition, null, out relativeMousePosition);

                    rectTransform.anchoredPosition = relativeMousePosition;
                }

                Image image = child.AddComponent<Image>();
                image.color = m_pinPanel.CurrentPinColor;

                Button button = child.AddComponent<Button>();
                // button.onClick.AddListener(() => OnButtonClick(mapPinID));

                child.AddComponent<MapPin>().scale = mapPinScale;
            }
        }
    }
}
