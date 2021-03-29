using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temperature : MonoBehaviour {
    [Tooltip("Maximum internal temperature value")]
    public float maxTemperature;

    [Tooltip("Player will start regenerating HP above this value (% of max temperature)")]
    [Range(0f, 1f)]
    public float maxTemperatureThreshold;

    [Tooltip("Player will start losing HP below this value (% of max temperature)")]
    [Range(0f, 1f)]
    public float minTemperatureThreshold;

    [Tooltip("Amount of HP regenerated per regen tick")]
    public int healthRegeneration;

    [Tooltip("Amount of time (in seconds) to wait between two regen ticks")]
    public float healthRegenerationCooldown;

    [Tooltip("Amount of HP lost per hypothermia tick")]
    public int damageHypothermia;

    [Tooltip("Amount of time (in seconds) to wait between two hypothermia ticks")]
    public float damageHypothermiaCooldown;

    [Tooltip("Amount of temperature regenerated per tick near an heat source")]
    public float temperatureRegeneration;

    [Tooltip("Amount of time (in seconds) to wait between two temperature regen ticks")]
    public float temperatureRegenerationCooldown = 1f;

    [Tooltip("Amount of time (in seconds) to wait between two temperature loss ticks")]
    public float temperatureLossCooldown = 1f;

    [Tooltip("[WIP] Blizzard loss rate (should be replaced by weather class)")]
    public float blizzardLossRate = 1f; // FIXME: Replace this once weather system is implemented

    private float m_currentTemperature = 0;
    private float m_healthRegenerationCooldownTimer = 0;
    private float m_damageHypothermiaCooldownTimer = 0;
    private float m_temperatureRegenerationCooldownTimer = 0;
    private float m_temperatureLossCooldownTimer = 0;

    // This variable contains the number of heat sources the player is currently in
    private int m_heatSources = 0;

    private GenericHealth m_health;
    private Inventory m_inventory;
    private Weather m_weather;
    private HUD m_playerHUD;

    void Start() {
        m_currentTemperature = maxTemperature;

        m_health = GetComponent<GenericHealth>();
        m_inventory = GetComponent<Inventory>();
        m_weather = GetComponent<Weather>();
        m_playerHUD = GetComponent<HUD>();
    }

    void Update() {
        // If the player is near at least one heat source, increase his temperature
        if (m_heatSources > 0) {
            // Increase our temperature regen timer
            m_temperatureRegenerationCooldownTimer += Time.deltaTime;

            // Process temperature regen and reset timer if its value is higher than cooldown
            if (m_temperatureRegenerationCooldownTimer > temperatureRegenerationCooldown) {
                m_temperatureRegenerationCooldownTimer = 0;

                m_currentTemperature += temperatureRegeneration;
            }

            if (m_currentTemperature > maxTemperature)
                m_currentTemperature = maxTemperature;
        }
        // Else, if the player is not near an heat source, decrease his temperature
        else {
            // Increase our temperature loss timer
            m_temperatureLossCooldownTimer += Time.deltaTime;

            // Process temperature loss and reset timer if its value is higher than cooldown
            if (m_temperatureLossCooldownTimer > temperatureLossCooldown) {
                m_temperatureLossCooldownTimer = 0;

                m_currentTemperature -= GetTemperatureLossRate();
            }

            if (m_currentTemperature < 0)
                m_currentTemperature = 0;
        }

        //---------------------------------------------------------------------
        // Process regen or hypothermia depending on current temperature
        //---------------------------------------------------------------------
        if (m_currentTemperature > maxTemperatureThreshold * maxTemperature) {
            // Increase our health regen timer
            m_healthRegenerationCooldownTimer += Time.deltaTime;

            // Process regen and reset timer if its value is higher than cooldown
            if (m_healthRegenerationCooldownTimer > healthRegeneration) {
                m_healthRegenerationCooldownTimer = 0;

                m_health.Heal(healthRegeneration);
            }
        }
        else if (m_currentTemperature < minTemperatureThreshold * maxTemperature) {
            // Increase our hypothermia timer
            m_damageHypothermiaCooldownTimer += Time.deltaTime;

            // Process damage and reset timer if its value is higher than cooldown
            if (m_damageHypothermiaCooldownTimer > damageHypothermiaCooldown) {
                m_damageHypothermiaCooldownTimer = 0;

                m_health.Hit(damageHypothermia);
            }
        }
        m_playerHUD.SetFrozen(Mathf.Clamp(m_currentTemperature / maxTemperature, 0, 1));
    }

    float GetTemperatureLossRate() {
        float clothType = 0f;
        Cloth cloth = m_inventory.GetCurrentCloth();
        if (cloth != null)
            clothType = (float)cloth.type;

        return (1 + (float)m_weather.CurrentWeather.blizzardStrength) - ((1 + (float)m_weather.CurrentWeather.blizzardStrength) * clothType / 100f);
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "HeatSource") {
            // If the player wasn't near an heat source, reset the timers
            if (m_heatSources < 1) {
                m_damageHypothermiaCooldownTimer = 0;
                m_healthRegenerationCooldownTimer = 0;
                m_temperatureRegenerationCooldownTimer = 0;
            }

            // Increase counter if we enter an heat source
            m_heatSources += 1;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.tag == "HeatSource") {
            // Decrease counter if we exit an heat source
            m_heatSources -= 1;

            // If the player is no longer near an heat source, reset the timers
            if (m_heatSources < 1) {
                m_damageHypothermiaCooldownTimer = 0;
                m_healthRegenerationCooldownTimer = 0;
                m_temperatureRegenerationCooldownTimer = 0;
            }

            if (m_heatSources < 0)
                Debug.LogError("Player Temperature: Heat sources counter is negative. That's not supposed to happen.");
        }
    }

    /// <summary>
    /// Reduce the current temperature from a given percentage.
    /// Primarly used when the player is attacked by an ennemy (Wolf for example).
    /// </summary>
    /// <param name="percentage">Percentage to deduce to the current temperature.</param>
    public void ReduceCurrentTemperatureWithPercentage(float percentage) {
        m_currentTemperature -= m_currentTemperature * percentage;
    }

    /// WARNING: For debug purpose only
    public float GetCurrentTemperature() {
        return m_currentTemperature;
    }
}
