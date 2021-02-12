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
        var weatherTypeNames = new Dictionary<WeatherPreset.EWeatherType, string>();
        weatherTypeNames.Add(WeatherPreset.EWeatherType.Cloudy, "Cloudy");
        weatherTypeNames.Add(WeatherPreset.EWeatherType.Rainy,  "Rainy");
        weatherTypeNames.Add(WeatherPreset.EWeatherType.Snowy,  "Snowy");
        weatherTypeNames.Add(WeatherPreset.EWeatherType.Sunny,  "Sunny");

        m_text.text = "Weather type: " + weatherTypeNames[weather.GetCurrentWeather().type] + " | Blizzard: " + weather.GetCurrentWeather().blizzardStrength;
    }
}
