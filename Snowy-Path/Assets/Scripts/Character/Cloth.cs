using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/Cloth")]
public class Cloth : ScriptableObject
{
    [System.Serializable]
    public enum EClothType {
        None = 0,
        Light = 20,
        Normal = 35,
        Heavy = 75
    }

    public EClothType type = EClothType.None;

    public float maxDurability = 240f;

    [Tooltip("Durability loss (per second)")]
    [Range(0f, 10f)]
    public float durabilityLoss = 1f;

    private float m_currentDurability = 0f;
    private float m_durabilityLossTimer = 0f;

    void Awake() {
        m_currentDurability = maxDurability;
    }

    /// <summary>
    /// Reduce the current durability from a given percentage.
    /// Primarly used when the player is attacked by an ennemy (Wolf for example).
    /// </summary>
    /// <param name="percentage">Percentage to deduce to the current durability</param>
    public void ReduceDurabilityPercentage(float percentage) {
        m_currentDurability -= m_currentDurability * percentage;
    }

    public float GetCurrentDurability() {
        return m_currentDurability;
    }

    public void ReduceDurability(WeatherPreset currentWeather) {
        if (m_durabilityLossTimer > 1f) {
            var d = durabilityLoss + ((maxDurability * durabilityLoss / 100f) * (float)currentWeather.durabilityReductionIntensity / 100f);
            // Debug.Log(d + " / sec | " + currentWeather.durabilityReductionIntensity.ToString() + " | " + currentWeather.blizzardStrength.ToString());
            m_currentDurability -= d;
            m_durabilityLossTimer = 0f;
        }
        else {
            m_durabilityLossTimer += Time.deltaTime;
        }
    }
}
