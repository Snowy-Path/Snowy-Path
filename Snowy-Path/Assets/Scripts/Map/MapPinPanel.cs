using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPinPanel : MonoBehaviour
{
    public Map map;
    public List<Color> pinTypes;

    private int m_currentPinType = 0;
    public int CurrentPinType { get { return m_currentPinType; } }
    public Color CurrentPinColor { get { return pinTypes[m_currentPinType]; } }

    void Start()
    {
        int i = 0;
        foreach (var pinColor in pinTypes) {
            // Initialize the object that will contain our map pin
            GameObject child = new GameObject("MapPin" + i);
            child.transform.parent = transform;

            RectTransform rectTransform = child.AddComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0f, 1f);
            rectTransform.anchorMax = new Vector2(0f, 1f);
            rectTransform.pivot = new Vector2(0f, 1f);
            rectTransform.anchoredPosition = new Vector3(i * 100, 0, 0);

            Image image = child.AddComponent<Image>();
            image.color = pinColor;

            int mapPinID = i;
            Button button = child.AddComponent<Button>();
            button.onClick.AddListener(() => OnButtonClick(mapPinID));

            ++i;
        }
    }

    void OnButtonClick(int i) {
        m_currentPinType = i;

        map.OnPinTypeChanged();
    }
}
