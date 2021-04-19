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

    // Update is called once per frame
    void Update()
    {
        
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

    public LightAndFogAsset LightTransitionTo(LightAndFogAsset lightAsset)
    {
        // Set Fog Rendering
        RenderSettings.fogDensity = lightAsset.fogDensity;
        RenderSettings.fogColor = lightAsset.fogColor;

        // Set Light Rendering
        RenderSettings.skybox = lightAsset.skyboxMaterial;
        RenderSettings.sun.enabled = lightAsset.activateSunSource;
        RenderSettings.reflectionIntensity = lightAsset.instensityMultiplier;

        return lightAsset;

    }
}
