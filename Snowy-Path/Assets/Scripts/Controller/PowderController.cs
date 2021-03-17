using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the powder effect when active. Holds every variable needed.
/// </summary>
public class PowderController : MonoBehaviour {

    #region Variables
    [Tooltip("Effectu duration (in seconds).")]
    public float effectDuration;

    [Tooltip("Color for lights.")]
    public Color effectColor;

    [Tooltip("Gradient colors for particle system.")]
    public Gradient particlesEffectGradient;

    [Tooltip("Torch light reference.")]
    public Light torchLight;
    private Color torchLightBaseColor;

    [Tooltip("Forever light reference.")]
    public Light foreverLight;
    private Color foreverLightBaseColor;

    [Tooltip("Particle system reference.")]
    public ParticleSystem torchParticles;

    private ParticleSystem.ColorOverLifetimeModule colorModule;
    private Gradient torchParticlesBaseGradient;

    private Coroutine effectCoroutine;
    private GameObject root;
    #endregion


    #region Built-In Methods
    /// <summary>
    /// Retrieves base color for lights and particle system.
    /// Find the "PowderEffectRoot" gameobject in the scene and deactivate it.
    /// </summary>
    private void Start() {
        torchLightBaseColor = torchLight.color;
        foreverLightBaseColor = foreverLight.color;

        colorModule = torchParticles.colorOverLifetime;
        torchParticlesBaseGradient = colorModule.color.gradient;

        root = GameObject.Find("PowderEffectRoot");
        root.SetActive(false);
    }
    #endregion


    #region Powder Effect Methods
    /// <summary>
    /// Activate the effect for <c>effectDuration</c> seconds.
    /// </summary>
    internal void ActivatePowderEffects() {
        if (effectCoroutine != null) {
            StopCoroutine(effectCoroutine);
        }
        effectCoroutine = StartCoroutine(ChangeColor());
    }

    /// <summary>
    /// Change the color and activate the objects dependent of the powder effect.
    /// </summary>
    private IEnumerator ChangeColor() {

        root.SetActive(true);
        torchLight.color = effectColor;
        foreverLight.color = effectColor;
        colorModule.color = particlesEffectGradient;

        yield return new WaitForSeconds(effectDuration);

        root.SetActive(false);
        torchLight.color = torchLightBaseColor;
        foreverLight.color = foreverLightBaseColor;
        colorModule.color = torchParticlesBaseGradient;

    }
    #endregion

}
