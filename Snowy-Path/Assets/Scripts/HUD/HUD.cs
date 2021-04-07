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
        SetBlood(Mathf.Clamp(playerHealth.CurrentHealth / (float)playerHealth.maxHealth, 0, 1));
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
        CalculateAlpha(ref bloodVignette, healthRatio, startBloodVignette);
        CalculateAlpha(ref bloodOverlay1, healthRatio, startBloodVignette);
        CalculateAlpha(ref bloodOverlay2, healthRatio, startBloodVignette);
        CalculateAlpha(ref bloodVein, healthRatio, startBloodVein);
    }

    public void SetFrozen(float temperatureRatio) {
        coldBreath = temperatureRatio < startBreath;
        CalculateAlpha(ref freeze1Overlay, temperatureRatio, startFreeze1);
        CalculateAlpha(ref freeze2Overlay, temperatureRatio, startFreeze2);

        if (temperatureRatio < startBlueOverlay) {
            float factor = blueColorMaxAlpha / startBlueOverlay;
            blueOverlay.SetAlpha(blueColorMaxAlpha - factor * temperatureRatio);
        }
        else
            blueOverlay.SetAlpha(0);
    }

    public void CalculateAlpha(ref Image img, float ratio, float startThreshold) {
        if (ratio < startThreshold) {
            float factor = 1 / startThreshold;
            img.SetAlpha(1 - factor * ratio);
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
