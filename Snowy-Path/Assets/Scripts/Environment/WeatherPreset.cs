using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Environment/Weather")]
public class WeatherPreset : ScriptableObject
{
    [System.Serializable]
    public enum EWeatherType {
        Sunny,
        Cloudy,
        Rainy,
        Snowy
    }

    [Tooltip("Weather type (currently unused)")]
    public EWeatherType type;

    [Tooltip("Blizzard strength (%)")]
    [Range(0f, 1f)]
    public float blizzardStrength = 0f;

    [Tooltip("Visibility (%)")]
    [Range(0f, 1f)]
    public float visibility = 1f;
}
