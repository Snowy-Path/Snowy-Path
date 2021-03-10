using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPinPanel : MonoBehaviour
{
    public Map map;
    public List<Color> pinTypes;
    public Sprite cursorTexture;

    private int m_currentPinType = 0;
    public int CurrentPinType { get { return m_currentPinType; } }
    public Color CurrentPinColor { get { return pinTypes[m_currentPinType]; } }

    private List<GameObject> m_mapPins = new List<GameObject>();
    public GameObject CurrentButton { get { return m_mapPins[m_currentPinType]; } } 

    private RectTransform m_cursor;

    void Start()
    {
        int i = 0;
        foreach (var pinColor in pinTypes) {
            // Initialize the object that will contain our map pin
            GameObject child = new GameObject("MapPin" + i);
            child.transform.parent = transform;

            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.localScale = Vector3.one;
            rectTransform.localPosition = Vector3.zero;
            rectTransform.anchorMin = new Vector2(0f, 1f);
            rectTransform.anchorMax = new Vector2(0f, 1f);
            rectTransform.pivot = new Vector2(0f, 1f);
            rectTransform.anchoredPosition = GetPinPosition(i);

            Image image = child.AddComponent<Image>();
            image.color = pinColor;

            int mapPinID = i;
            Button button = child.AddComponent<Button>();
            button.onClick.AddListener(() => OnButtonClick(mapPinID));

            m_mapPins.Add(child);

            ++i;
        }

        CreateCursor();
    }

    void CreateCursor() {
        GameObject cursor = new GameObject("pinCursor");
        cursor.transform.parent = transform;

        RectTransform rectTransform = cursor.AddComponent<RectTransform>();
        rectTransform.localScale = Vector3.one / 2f;
        rectTransform.localPosition = Vector3.zero;
        rectTransform.anchorMin = new Vector2(0f, 1f);
        rectTransform.anchorMax = new Vector2(0f, 1f);
        rectTransform.pivot = new Vector2(0f, 1f);
        rectTransform.anchoredPosition = Vector3.zero;

        Image img = cursor.AddComponent<Image>();
        img.color = new Color(1f, 0f, 1f, 0.5f);
        img.sprite = cursorTexture;

        m_cursor = rectTransform;
    }

    void OnButtonClick(int i) {
        m_currentPinType = i;

        map.OnPinTypeChanged();
    }

    Vector2 GetPinPosition(int pinType) {
        return new Vector2((pinType % 5) * 100f, -(pinType / 5) * 100f);
    }

    public void SelectPinType(int pinType) {
        m_currentPinType = pinType;
        m_cursor.anchoredPosition = GetPinPosition(m_currentPinType);
        map.OnPinTypeChanged();
    }

    public void SelectNextPinType() {
        m_currentPinType = (m_currentPinType + 1) % m_mapPins.Count;
        m_cursor.anchoredPosition = GetPinPosition(m_currentPinType);
        map.OnPinTypeChanged();
    }
}
