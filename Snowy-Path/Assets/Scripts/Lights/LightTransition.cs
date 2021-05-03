using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTransition : MonoBehaviour
{
    public LightAndFogAsset lightFogAsset_1;
    public LightAndFogAsset lightFogAsset_2;
    LightAndFogAsset activeLightAsset;

    private void Start()
    {
        activeLightAsset = lightFogAsset_1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchLight();
        }
    }

    public void SwitchLight()
    {
        activeLightAsset = (lightFogAsset_1 == activeLightAsset) ? LightTransitionTo(lightFogAsset_2) : LightTransitionTo(lightFogAsset_1);
    }

    public static LightAndFogAsset LightTransitionTo(LightAndFogAsset lightAsset)
    {
        // Set Fog Rendering
        RenderSettings.fogDensity = lightAsset.fogDensity;
        RenderSettings.fogColor = lightAsset.fogColor;

        // Set Environement Light Setting
        switch (lightAsset.environementalSource)    
        {
            case UnityEngine.Rendering.AmbientMode.Skybox:
                RenderSettings.ambientMode = lightAsset.environementalSource;
                RenderSettings.ambientIntensity = lightAsset.intensityMultiplierEnviro;

                break;
            case UnityEngine.Rendering.AmbientMode.Trilight:
                RenderSettings.ambientMode = lightAsset.environementalSource;
                RenderSettings.ambientEquatorColor = lightAsset.equatorColor;
                RenderSettings.ambientGroundColor = lightAsset.groundColor;
                RenderSettings.ambientSkyColor = lightAsset.skyColor;

                break;
            case UnityEngine.Rendering.AmbientMode.Flat:
                RenderSettings.ambientMode = lightAsset.environementalSource;

                break;
            case UnityEngine.Rendering.AmbientMode.Custom:
                RenderSettings.ambientMode = lightAsset.environementalSource;

                break;
        }


        // Set Light Rendering
        RenderSettings.skybox = lightAsset.skyboxMaterial;
        if(RenderSettings.sun != null)
        {
            RenderSettings.sun.enabled = lightAsset.activateSunSource;
        }
        RenderSettings.reflectionIntensity = lightAsset.intensityMultiplierReflect;

        return lightAsset;

    }
}
