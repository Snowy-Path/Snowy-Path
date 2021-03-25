using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public WeatherPreset defaultWeather;

    private Dictionary<int, WeatherZone> activeWeatherZones = new Dictionary<int, WeatherZone>();
    private Inventory m_inventory;

    private WeatherPreset m_currentWeather;
    public WeatherPreset CurrentWeather {
        get { return m_currentWeather; }
        private set {
            if (value.blizzardStrength != m_currentWeather.blizzardStrength) {
                blizzardAnimator.SetFloat("BlizzardStrength", value.blizzardStrength);
                blizzardAnimator.SetTrigger("BlizzardChanged");
            }
            m_currentWeather = value;
        }
    }

    public Animator blizzardAnimator;

    private void Awake() {
        m_currentWeather = defaultWeather;
    }

    void Start() {
        m_inventory = GetComponent<Inventory>();
    }

    void Update() {
        m_inventory.ReduceClothDurability(GetCurrentWeather());
    }

    public WeatherPreset GetCurrentWeather() {
        int id = 0;
        int highestBlizzardStrength = 0;
        bool hasFoundWeather = false;

        // Try to find the weather zone with the highest blizzard strength
        // Only that one will be considered as the current weather
        foreach (var pair in m_activeWeatherZones) {
            if ((int)pair.Value.weatherPreset.blizzardStrength > highestBlizzardStrength) {
                highestBlizzardStrength = (int)pair.Value.weatherPreset.blizzardStrength;
                id = pair.Key;
                hasFoundWeather = true;
            }
        }

        // If we found a weather zone then return it, else return the default weather
        if (hasFoundWeather)
            return m_activeWeatherZones[id].weatherPreset;
        else
            return defaultWeather;
    }

    void OnTriggerEnter(Collider other) {
        // If we enter a weather zone, add it to our dictionary
        if (other.tag == "WeatherZone") {
            var weatherZone = other.GetComponentInParent<WeatherZone>();
            activeWeatherZones.Add(weatherZone.GetInstanceID(), weatherZone);
            CurrentWeather = GetCurrentWeather();
        }
    }

    void OnTriggerExit(Collider other) {
        // If we exit a weather zone, remove it from our dictionary
        if (other.tag == "WeatherZone") {
            var weatherZone = other.GetComponentInParent<WeatherZone>();
            activeWeatherZones.Remove(weatherZone.GetInstanceID());
            CurrentWeather = GetCurrentWeather();
        }
    }
}
