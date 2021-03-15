using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPinManager : MonoBehaviour
{
    private Map m_map;
    private MapUI m_mapUI;
    private RectTransform m_rectTransform;

    private bool m_isEditingPin = false;
    private GameObject m_lastPinPlaced;
    private Vector3 m_lastPinPosition;
    private Color m_lastPinColor;

    void Start()
    {
        m_map = GetComponent<Map>();
        m_mapUI = GetComponentInParent<MapUI>();
        m_rectTransform = GetComponent<RectTransform>();

        m_mapUI.PinPanel.transform.Find("Confirm").gameObject.GetComponent<Button>().onClick.AddListener(() => PinConfirm());
        m_mapUI.PinPanel.transform.Find("Remove").gameObject.GetComponent<Button>().onClick.AddListener(() => PinRemove());
        m_mapUI.PinPanel.transform.Find("Cancel").gameObject.GetComponent<Button>().onClick.AddListener(() => PinCancel());
    }

    public void PinConfirm()
    {
        m_isEditingPin = false;
        m_lastPinPlaced = null;

        m_mapUI.PinPanel.SelectPinType(0);
        m_mapUI.ClosePinPanel();
    }

    public void PinRemove()
    {
        m_isEditingPin = false;
        Destroy(m_lastPinPlaced);
        m_lastPinPlaced = null;

        m_mapUI.PinPanel.SelectPinType(0);
        m_mapUI.ClosePinPanel();
    }

    public void PinCancel()
    {
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

        m_mapUI.PinPanel.SelectPinType(0);
        m_mapUI.ClosePinPanel();
    }

    // Click handler for placed pins
    void OnPinClick(GameObject pin)
    {
        m_isEditingPin = true;
        m_lastPinPosition = pin.transform.position;
        m_lastPinPlaced = pin;
        m_lastPinColor = pin.GetComponent<Image>().color;

        m_mapUI.PinPanel.SelectPinType(pin.GetComponent<MapPin>().pinType);
        m_mapUI.OpenPinPanel();
    }

    // Used by MapPinPanel
    public void OnPinTypeChanged()
    {
        if (m_lastPinPlaced != null) {
            m_lastPinPlaced.GetComponent<Image>().color = m_mapUI.PinPanel.CurrentPinColor;
            m_lastPinPlaced.GetComponent<MapPin>().pinType = m_mapUI.PinPanel.CurrentPinType;
        }
    }

    public void PlacePin(Vector2 position)
    {
        if (m_lastPinPlaced != null) {
            Destroy(m_lastPinPlaced);
            m_lastPinPlaced = null;
        }

        m_mapUI.OpenPinPanel();

        int currentPinType = m_mapUI.PinPanel.CurrentPinType;

        GameObject child = new GameObject("MapPin");
        child.transform.parent = transform;

        RectTransform rectTransform = child.AddComponent<RectTransform>();
        rectTransform.localScale = new Vector3(
            m_mapUI.mapPinScale / transform.localScale.x,
            m_mapUI.mapPinScale / transform.localScale.y,
            m_mapUI.mapPinScale / transform.localScale.z
        );

        Vector2 relativePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_rectTransform, position, null, out relativePosition);

        rectTransform.anchoredPosition = relativePosition;

        Image image = child.AddComponent<Image>();
        image.color = m_mapUI.PinPanel.CurrentPinColor;

        Button button = child.AddComponent<Button>();
        button.onClick.AddListener(() => OnPinClick(child));

        MapPin pin = child.AddComponent<MapPin>();
        pin.scale = m_mapUI.mapPinScale;
        pin.pinType = m_mapUI.PinPanel.CurrentPinType;

        m_lastPinPlaced = child;
    }
}
