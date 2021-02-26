using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MapUIController : MonoBehaviour
{
    public float mapPinScale = 1f;
    public InputActionAsset m_inputActionAsset;

    private MapUI m_mapUI;
    private MapPinPanel m_pinPanel;
    private bool m_canPlacePin = false;
    private bool m_isPinModeEnabled = false;

    void Start()
    {
        m_mapUI = GetComponentInChildren<MapUI>();
        m_pinPanel = GetComponentInChildren<MapPinPanel>();
        if (m_mapUI == null)
            Debug.LogError("[MapUIController] Can't find MapUI.");
        if (m_pinPanel == null)
            Debug.LogError("[MapUIController] Can't find map pin panel.");

        m_pinPanel.gameObject.SetActive(false);
    }

    void Update()
    {
        InputAction moveAction = m_inputActionAsset.FindAction("UI/Map Move");
        if (moveAction.triggered)
            m_canPlacePin = false;
    }

    public void OnPinPlaced(InputAction.CallbackContext context)
    {
        if (context.started)
            m_canPlacePin = true;

        if (m_canPlacePin && context.canceled) {
            int currentPinType = m_pinPanel.CurrentPinType;
            if (!m_isPinModeEnabled) {
                m_isPinModeEnabled = true;
                m_pinPanel.gameObject.SetActive(true);
            }

            // FIXME: When m_isPinModeEnabled, and for all the duration of the mode, the same pin should be edited instead of creating new ones each time

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
