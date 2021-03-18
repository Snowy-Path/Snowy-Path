using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour {

    [SerializeField]
    private WeatherPreset defaultWeather;

    [SerializeField]
    private Animator animator;

    private Dictionary<int, WeatherZone> activeWeatherZones = new Dictionary<int, WeatherZone>();

    private WeatherPreset m_currentWeather;
    public WeatherPreset CurrentWeather {
        get {
            return m_currentWeather;
        }
        private set {
            m_currentWeather = value;
            animator.SetFloat("BlizzardStrength", m_currentWeather.blizzardStrength);
            animator.SetTrigger("BlizzardChanged");
        }
    }

    private void Awake() {
        CurrentWeather = defaultWeather;
    }

    void OnTriggerEnter(Collider other) {
        // If we enter a weather zone, add it to our dictionary
        if (other.tag == "WeatherZone") {
            var weatherZone = other.GetComponentInParent<WeatherZone>();
            activeWeatherZones.Add(weatherZone.GetInstanceID(), weatherZone);
            if (weatherZone.weatherPreset.blizzardStrength > CurrentWeather.blizzardStrength) {
                CurrentWeather = weatherZone.weatherPreset;
            }
        }
    }

    void OnTriggerExit(Collider other) {
        // If we exit a weather zone, remove it from our dictionary
        if (other.tag == "WeatherZone") {
            var weatherZone = other.GetComponentInParent<WeatherZone>();
            activeWeatherZones.Remove(weatherZone.GetInstanceID());
            FindHighestWeatherZone();
        }
    }

    private void FindHighestWeatherZone() {

        // Guard
        if (activeWeatherZones.Count == 0) {
            CurrentWeather = defaultWeather;
            return;
        }

        int id = 0;
        float highestBlizzardStrength = 0;

        foreach (var pair in activeWeatherZones) {
            if (pair.Value.weatherPreset.blizzardStrength > highestBlizzardStrength) {
                highestBlizzardStrength = pair.Value.weatherPreset.blizzardStrength;
                id = pair.Key;
            }
        }

        // Guard
        if (CurrentWeather == activeWeatherZones[id].weatherPreset) {
            return;
        }

        CurrentWeather = activeWeatherZones[id].weatherPreset;
    }

}