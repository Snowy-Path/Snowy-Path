using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using UnityEngine.Rendering;

public class HUD : MonoBehaviour {

    [Header("Stamina")]
    [SerializeField] AnimationCurve staminaCurve;

    [Header("Breath")]
    [Range(0, 1)] [SerializeField] float startBreath = 0.5f;


    [Header("Freeze")]
    [Tooltip("% of max temperature")]
    [Range(0, 1)] [SerializeField] float startFreeze1 = 0.7f;
    [Tooltip("% of max temperature")]
    [Range(0, 1)] [SerializeField] float startFreeze2 = 0.2f;

    [Header("Blood")]
    [Range(0, 10)] [SerializeField] int startBloodVignette = 2;
    [Range(0, 10)] [SerializeField] int startBlood1 = 1;
    [Range(0, 10)] [SerializeField] int startBlood2 = 1;
    [Range(0, 10)] [SerializeField] int startBloodVein = 2;

    [Range(0, 1)] [SerializeField] float bloodHitStep = 0.1f;
    [Range(0, 1)] [SerializeField] float bloodHealStep = 0.1f;

    [Header("Set up")]
    [SerializeField] Image bloodVignette;
    [SerializeField] Image bloodOverlay1;
    [SerializeField] Image bloodOverlay2;
    [SerializeField] Image bloodVein;
    [SerializeField] Image freeze1Overlay;
    [SerializeField] Image freeze2Overlay;
    [SerializeField] private VisualEffect breathEffect;

    private GenericHealth playerHealth;
    private Temperature temperature;

    private void Start() {
        ResetOverlays();
        playerHealth = GetComponentInParent<GenericHealth>();
        temperature = GetComponentInParent<Temperature>();
    }

    private void Update() {
        float tempRatio = Mathf.Clamp(temperature.CurrentTemperature / temperature.maxTemperature, 0, 1);
        SetFreezeOverlays(tempRatio);
        SetBloodOverlays();
    }

    public void ResetOverlays() {
        bloodVignette.SetAlpha(0);
        bloodOverlay1.SetAlpha(0);
        bloodOverlay2.SetAlpha(0);
        bloodVein.SetAlpha(0);
        freeze1Overlay.SetAlpha(0);
        freeze2Overlay.SetAlpha(0);
    }

    /// <summary>
    /// Update blood overlays alpha, called on player hit
    /// </summary>
    public void SetBloodOverlays() {
        int health = playerHealth.GetCurrentHealth();
        CalculateBloodAlpha(ref bloodVignette, health, startBloodVignette);
        CalculateBloodAlpha(ref bloodOverlay1, health, startBlood1);
        CalculateBloodAlpha(ref bloodOverlay2, health, startBlood2);
        CalculateBloodAlpha(ref bloodVein, health, startBloodVein);
    }

    private void CalculateBloodAlpha(ref Image image, int healthValue, int start) {
        if (healthValue <= start) {
            StartCoroutine(FadeAlpha(image, 1, bloodHitStep));
        }
        else
            StartCoroutine(FadeAlpha(image, 0, -bloodHealStep));
    }

    private IEnumerator FadeAlpha(Image image, float targetAlpha, float step) {
        targetAlpha = Mathf.Clamp(targetAlpha, 0, 1);

        if (step > 0) {
            while (image.color.a < targetAlpha) {
                image.SetAlpha(image.color.a + step);
                yield return new WaitForSeconds(0.05f);
            }
        }
        else if (step < 0) {
            while (image.color.a > targetAlpha) {
                image.SetAlpha(image.color.a + step);
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
            Debug.LogError("HUD blood fade step can't be 0");
    }

    public void SetFreezeOverlays(float temperatureRatio) {
        //Breath
        if (temperatureRatio < startBreath) {
            breathEffect.enabled = true;
        }
        else {
            breathEffect.enabled = false;
        }


        CalculateFreezeAlpha(ref freeze1Overlay, temperatureRatio, startFreeze1);
        CalculateFreezeAlpha(ref freeze2Overlay, temperatureRatio, startFreeze2);
    }

    public void CalculateFreezeAlpha(ref Image img, float ratio, float startThreshold) {
        if (ratio < startThreshold) {
            float factor = 1 / startThreshold;
            float newAlpha = Mathf.Lerp(img.color.a, 1 - factor * ratio, Time.deltaTime);

            img.SetAlpha(newAlpha);
        }
        else
            img.SetAlpha(0);
    }
}

public static class HUDHelper {
    public static void SetAlpha(this Image img, float alpha) {
        Color c;
        c = img.color;
        c.a = Mathf.Clamp(alpha, 0, 1);
        img.color = c;
    }
}
