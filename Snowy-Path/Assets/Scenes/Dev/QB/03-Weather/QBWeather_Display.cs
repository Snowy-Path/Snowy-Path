using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QBWeather_Display : MonoBehaviour
{
    public Weather weather;

    private Text m_text;

    void Start()
    {
        m_text = GetComponent<Text>();
    }

    void Update()
    {
        m_text.text = "Blizzard: " + weather.GetCurrentWeather().blizzardStrength;
    }
}
