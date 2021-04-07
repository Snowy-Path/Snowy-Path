using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    [Header("HUD parameters")]
    [Range(0, 1)] [SerializeField] float startBreath = 0.5f;

    [Range(0, 1)] [SerializeField] float startFreeze1 = 0.5f;
    [Range(0, 1)] [SerializeField] float startFreeze2 = 0.3f;

    [Range(0, 1)] [SerializeField] float startBloodVignette = 1f;
    [Range(0, 1)] [SerializeField] float startBlood1 = 0.3f;
    [Range(0, 1)] [SerializeField] float startBlood2 = 0.3f;
    [Range(0, 1)] [SerializeField] float startBloodVein = 0.3f;

    [Range(0, 1)] [SerializeField] float startBlueOverlay = 0.4f;
    [Range(0, 1)] [SerializeField] float blueColorMaxAlpha = 0.3f;

    [Header("Debug")]
    [SerializeField] bool debugMode;
    [Range(0, 1)] [SerializeField] float health;
    [Range(0, 1)] [SerializeField] float temperature;
    [Range(0, 1)] [SerializeField] float stamina;

    [Header("Set up")]
    [SerializeField] Image staminaOverlay;
    [SerializeField] Image bloodVignette;
    [SerializeField] Image bloodOverlay1;
    [SerializeField] Image bloodOverlay2;
    [SerializeField] Image bloodVein;
    [SerializeField] Image freeze1Overlay;
    [SerializeField] Image freeze2Overlay;
    [SerializeField] Image blueOverlay;
    [SerializeField] Image breathOverlay;

    [SerializeField] AnimationCurve staminaCurve;
    [SerializeField] AnimationCurve breathCurve;

    private GenericHealth playerHealth;
    private PlayerController controller;
    private bool coldBreath = false;
    private float breathTimer;
    private int breathDiv = 6;

    private void Start() {
        ResetOverlays();
        playerHealth = GetComponent<GenericHealth>();
        controller = GetComponent<PlayerController>();
    }

    private void Update() {
     
        if (debugMode) {
            SetBlood(health);
            SetFrozen(temperature);
            SetStamina(stamina);
            return;
        }

        SetBlood(1 - Mathf.Clamp(playerHealth.CurrentHealth / (float)playerHealth.maxHealth, 0, 1));
        ColdBreath();
    }

    public void ResetOverlays() {
        staminaOverlay.SetAlpha(0);
        bloodVignette.SetAlpha(0);
        bloodOverlay1.SetAlpha(0);
        bloodOverlay2.SetAlpha(0);
        bloodVein.SetAlpha(0);
        freeze1Overlay.SetAlpha(0);
        freeze2Overlay.SetAlpha(0);
        blueOverlay.SetAlpha(0);
        breathOverlay.SetAlpha(0);
    }

    public void ColdBreath() {
        float breathSpeed = controller.CurrentSpeed / breathDiv;

        breathTimer += Time.deltaTime * breathSpeed;
        if (breathTimer > breathCurve.keys[breathCurve.keys.Length - 1].time)
            breathTimer = 0;
        if (coldBreath) {
            breathOverlay.SetAlpha(breathCurve.Evaluate(breathTimer));
        }
        else
            breathOverlay.SetAlpha(0);
    }

    public void SetStamina(float staminaRatio) {
        staminaOverlay.SetAlpha(staminaCurve.Evaluate(staminaRatio));
    }

    public void SetBlood(float healthRatio) {
        healthRatio = 1 - healthRatio;
        CalculateAlpha(ref bloodVignette, healthRatio, startBloodVignette);
        CalculateAlpha(ref bloodOverlay1, healthRatio, startBloodVignette);
        CalculateAlpha(ref bloodOverlay2, healthRatio, startBloodVignette);
        CalculateAlpha(ref bloodVein, healthRatio, startBloodVein);
    }

    public void SetFrozen(float temperatureRatio) {
        coldBreath = temperatureRatio < startBreath;
        temperatureRatio = 1 - temperatureRatio;
        CalculateAlpha(ref freeze1Overlay, temperatureRatio, startFreeze1);
    }

    public void CalculateAlpha(ref Image img, float amount, float startThreshold) {
        float k = 1 / startThreshold;
        img.SetAlpha(k * amount);
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
