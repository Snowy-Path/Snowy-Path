using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    public WeatherPreset defaultWeather;

    [SerializeField]
    private FMODUnity.StudioEventEmitter m_blizzardEmitter;
    private FMOD.Studio.PARAMETER_ID m_blizzardChangeID;

    private Dictionary<int, WeatherZone> m_activeWeatherZones = new Dictionary<int, WeatherZone>();
    private Inventory m_inventory;

    private WeatherPreset m_currentWeather;
    public WeatherPreset CurrentWeather {
        get { return m_currentWeather; }
        private set {
            //if (value.blizzardStrength != m_currentWeather.blizzardStrength) {
            //    blizzardAnimator.SetFloat("BlizzardStrength", value.blizzardStrength);
            //    blizzardAnimator.SetTrigger("BlizzardChanged");
            //}
            m_currentWeather = value;
            SwitchBlizzardSoundParametter();
        }
    }

    //public Animator blizzardAnimator;

    private void Awake() {
        m_currentWeather = defaultWeather;
    }

    void Start() {
        m_inventory = GetComponent<Inventory>();

        FMOD.Studio.EventDescription blizzardChangeEventDesc;
        m_blizzardEmitter.EventInstance.getDescription(out blizzardChangeEventDesc);
        FMOD.Studio.PARAMETER_DESCRIPTION blizzardChangeParametterDesc;
        blizzardChangeEventDesc.getParameterDescriptionByName("Blizzard Changed", out blizzardChangeParametterDesc);
        m_blizzardChangeID = blizzardChangeParametterDesc.id;
    }

    void Update() {
        m_inventory.ReduceClothDurability(GetCurrentWeather());
    }

    private void SwitchBlizzardSoundParametter() {
        switch (m_currentWeather.blizzardStrength) {
            case WeatherPreset.EBlizzardStrength.None:
                m_blizzardEmitter.SetParameter(m_blizzardChangeID, 1.0f);
                break;
            case WeatherPreset.EBlizzardStrength.Low:
                m_blizzardEmitter.SetParameter(m_blizzardChangeID, 0.1f);
                break;
            case WeatherPreset.EBlizzardStrength.Medium:
                m_blizzardEmitter.SetParameter(m_blizzardChangeID, 0.4f);
                break;
            case WeatherPreset.EBlizzardStrength.High:
                m_blizzardEmitter.SetParameter(m_blizzardChangeID, 0.6f);
                break;
        }
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
            var weatherZone = other.GetComponent<WeatherZone>();
            if (!m_activeWeatherZones.ContainsKey(weatherZone.GetInstanceID()))
            {
                m_activeWeatherZones.Add(weatherZone.GetInstanceID(), weatherZone);
            }
            CurrentWeather = GetCurrentWeather();
        }
    }

    void OnTriggerExit(Collider other) {
        // If we exit a weather zone, remove it from our dictionary
        if (other.tag == "WeatherZone") {
            var weatherZone = other.GetComponent<WeatherZone>();
            m_activeWeatherZones.Remove(weatherZone.GetInstanceID());
            CurrentWeather = GetCurrentWeather();
        }
    }

    internal void ResetBlizzard() {
        CurrentWeather = defaultWeather;
    }
}
