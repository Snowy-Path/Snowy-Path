using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

    [Header("HUD parameters")]
    [SerializeField] float startFreeze1 = 0.3f;
    [SerializeField] float startFreeze2 = 0.3f;
    [SerializeField] float startBlood1 = 0.3f;
    [SerializeField] float startBlood2 = 0.3f;
    [SerializeField] float startBlueOverlay = 0.4f;
    [SerializeField] float startBreath = 0.5f;
    [SerializeField] float blueColorMaxAlpha = 0.3f;

    [Header("Set up")]
    [SerializeField] Image staminaOverlay;
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

 

    //private const float freezeMaxAlpha = 0.2f;

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

        if (amount < startFreeze) {
            freezeColor.a = (startFreeze - amount) * freezeMaxAlpha / startFreeze;
        }
        else
            freezeColor.a = 0;

        if (amount < startBlueOverlay) {
            blueColor.a = (startBlueOverlay - amount) * blueColorMaxAlpha / startBlueOverlay;
        }
        else
            blueColor.a = 0;

        coldBreath = amount < startBreath;

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
