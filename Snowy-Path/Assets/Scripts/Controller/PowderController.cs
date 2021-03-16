using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowderController : MonoBehaviour {

    #region Variables
    public Color effectColor;
    public Gradient particlesEffectGradient;

    public Light torchLight;
    private Color torchLightBaseColor;

    public Light foreverLight;
    private Color foreverLightBaseColor;

    public ParticleSystem torchParticles;
    private ParticleSystem.ColorOverLifetimeModule colorModule;
    private Gradient torchParticlesBaseGradient;

    private Coroutine effectCoroutine;

    public float effectDuration;
    #endregion


    #region Built-In Methods
    private void Start() {
        torchLightBaseColor = torchLight.color;
        foreverLightBaseColor = foreverLight.color;

        colorModule = torchParticles.colorOverLifetime;
        torchParticlesBaseGradient = colorModule.color.gradient;
    }
    #endregion


    #region Powder Effect Methods
    internal void ActivatePowderEffects() {
        if (effectCoroutine != null) {
            StopCoroutine(effectCoroutine);
        }
        effectCoroutine = StartCoroutine(ChangeColor());
    }

    private IEnumerator ChangeColor() {

        torchLight.color = effectColor;
        foreverLight.color = effectColor;
        colorModule.color = particlesEffectGradient;

        yield return new WaitForSeconds(effectDuration);

        torchLight.color = torchLightBaseColor;
        foreverLight.color = foreverLightBaseColor;
        colorModule.color = torchParticlesBaseGradient;

    }
    #endregion

}
