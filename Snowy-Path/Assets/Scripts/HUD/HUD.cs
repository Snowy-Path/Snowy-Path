using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    [SerializeField] Image staminaOverlay;
    [SerializeField] Image bloodOverlay;
    [SerializeField] Image freezeOverlay;
    [SerializeField] Image breathOverlay;
    [SerializeField] Image blueOverlay;

    [SerializeField] AnimationCurve staminaCurve;
    [SerializeField] AnimationCurve breathCurve;

    private GenericHealth playerHealth;
    private PlayerController controller;
    private bool coldBreath = false;
    private float breathTimer;

    private const int breathDiv = 6;
    private const float freezeThreshold = 0.3f;
    private const float blueColorThreshold = 0.4f;
    private const float breathThreshold = 0.5f;

    private const float freezeMaxAlpha = 0.2f;
    private const float blueColorMaxAlpha = 0.3f;

    private void Start() {
        ResetOverlays();
        playerHealth = GetComponent<GenericHealth>();
        controller = GetComponent<PlayerController>();
    }

    private void Update() {
        SetBlood(1 - Mathf.Clamp(playerHealth.CurrentHealth / (float)playerHealth.maxHealth, 0, 1));
        ColdBreath();
    }

    public void ResetOverlays() {
        ResetAlpha(ref staminaOverlay);
        ResetAlpha(ref bloodOverlay);
        ResetAlpha(ref freezeOverlay);
        ResetAlpha(ref breathOverlay);
        ResetAlpha(ref blueOverlay);
    }

    public void ColdBreath() {
        Color breathColor = breathOverlay.color;
        float breathSpeed = controller.CurrentSpeed / breathDiv;

        breathTimer += Time.deltaTime * breathSpeed;
        if (breathTimer > breathCurve.keys[breathCurve.keys.Length - 1].time)
            breathTimer = 0;
        if (coldBreath) {
            breathColor.a = breathCurve.Evaluate(breathTimer);
        }
        else
            breathColor.a = 0;

        breathOverlay.color = breathColor;
    }

    public void SetStamina(float amount) {
        Color color = staminaOverlay.color;
        color.a = Mathf.Clamp(staminaCurve.Evaluate(amount), 0, 1);
        staminaOverlay.color = color;
    }

    public void SetBlood(float amount) {
        Color color = bloodOverlay.color;
        color.a = amount;
        bloodOverlay.color = color;
    }

    public void SetFrozen(float amount) {
        Color freezeColor = freezeOverlay.color;
        Color blueColor = blueOverlay.color;

        if (amount < freezeThreshold) {
            freezeColor.a = (freezeThreshold - amount) * freezeMaxAlpha / freezeThreshold;
        }
        else
            freezeColor.a = 0;

        if (amount < blueColorThreshold) {
            blueColor.a = (blueColorThreshold - amount) * blueColorMaxAlpha / blueColorThreshold;
        }
        else
            blueColor.a = 0;

        coldBreath = amount < breathThreshold;

        freezeOverlay.color = freezeColor;
        blueOverlay.color = blueColor;
    }

    private void ResetAlpha(ref Image image) {
        Color c;
        c = image.color;
        c.a = 0;
        image.color = c;
    }
}
