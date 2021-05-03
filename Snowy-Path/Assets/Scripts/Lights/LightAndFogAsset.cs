using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "LightAndFogAsset", menuName = "LightFog/LightFog")]
public class LightAndFogAsset : ScriptableObject
{
    [Header("Fog Settings")]
    public Color fogColor;
    [Range(0f, 1f)]
    public float fogDensity = 0;

    [Header("Light Skybox Settings")]
    public Material skyboxMaterial;
    public bool activateSunSource;
    [Range(0f, 1f)]
    public float intensityMultiplierReflect = 0;

    [Header("Enviro light settings")]
    public AmbientMode environementalSource;

    [Header("Skybox")]
    [Range(0f, 8f)]
    public float intensityMultiplierEnviro = 1;

    [Header("Gradient")]
    public Color skyColor;
    public Color equatorColor;
    public Color groundColor;



    //public Material skyboxMaterial;
    //public bool activateSunSource;
    //[Range(0f, 1f)]
    //public float instensityMultiplier = 0;



}
