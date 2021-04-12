﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Environment/Weather")]
public class WeatherPreset : ScriptableObject
{
    [System.Serializable]
    public enum EBlizzardStrength {
        None = 0,
        Low = 25,
        Medium = 60,
        High = 200
    };

    public EBlizzardStrength blizzardStrength = EBlizzardStrength.None;

    [System.Serializable]
    public enum EDurabilityReductionIntensity {
        None = 0,
        Low = 15,
        Medium = 30,
        High = 50
    };

    [System.NonSerialized]
    public EDurabilityReductionIntensity durabilityReductionIntensity = EDurabilityReductionIntensity.None;

    [Tooltip("Visibility (%)")]
    [Range(0f, 1f)]
    public float visibility = 1f;

    void Awake() {
        // Initialize durabilityReductionIntensity depending on blizzardStrength value
        Dictionary<EBlizzardStrength, EDurabilityReductionIntensity> m = new Dictionary<EBlizzardStrength, EDurabilityReductionIntensity>();
        m.Add(EBlizzardStrength.None, EDurabilityReductionIntensity.None);
        m.Add(EBlizzardStrength.Low, EDurabilityReductionIntensity.Low);
        m.Add(EBlizzardStrength.Medium, EDurabilityReductionIntensity.Medium);
        m.Add(EBlizzardStrength.High, EDurabilityReductionIntensity.High);
        durabilityReductionIntensity = m[blizzardStrength];
    }
}
