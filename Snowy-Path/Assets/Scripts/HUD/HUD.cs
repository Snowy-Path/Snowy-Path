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

    private GenericHealth playerHealth;

    private const float freezeThreshold = 0.3f;
    private const float blueColorThreshold = 0.4f;
    private const float breathThreshold = 0.5f;

    private const float freezeMaxAlpha = 0.2f;
    private const float blueColorMaxAlpha = 0.3f;
    private const float breathMaxAlpha = 0.6f;

    private void Start() {
        ResetOverlays();
        playerHealth = GetComponent<GenericHealth>();
    }

    private void Update() {
        SetBlood(1 - Mathf.Clamp(playerHealth.CurrentHealth / (float)playerHealth.maxHealth, 0, 1));
    }

    public void ResetOverlays() {
        ResetAlpha(ref staminaOverlay);
        ResetAlpha(ref bloodOverlay);
        ResetAlpha(ref freezeOverlay);
        ResetAlpha(ref breathOverlay);
        ResetAlpha(ref blueOverlay);
    }



    public void SetStamina(float amount) {
        Color color = staminaOverlay.color;
        color.a = Mathf.Clamp(staminaCurve.Evaluate(amount), 0, 1);
        Debug.Log(color.a);
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
        Color breathColor = breathOverlay.color;

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

        if (amount < breathThreshold) {
            breathColor.a = (breathThreshold - amount) * breathMaxAlpha / breathThreshold;
        }
        else
            breathColor.a = 0;

        freezeOverlay.color = freezeColor;
        blueOverlay.color = blueColor;
        breathOverlay.color = breathColor;
    }

    private void ResetAlpha(ref Image image) {
        Color c;
        c = image.color;
        c.a = 0;
        image.color = c;
    }
}
