using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LightAndFogAsset", menuName = "LightFog/LightFog")]
public class LightAndFogAsset : ScriptableObject
{
    [Header("Fog Settings")]
    public Color fogColor;
    [Range(0f, 1f)]
    public float fogDensity = 0;

    [Header("Light Settings")]
    public Material skyboxMaterial;
    public bool activateSunSource;
    [Range(0f, 1f)]
    public float instensityMultiplier = 0;

    

}
