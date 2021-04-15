using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

public class HUD : MonoBehaviour {

    [Header("Stamina")]
    [SerializeField] AnimationCurve staminaCurve;

    [Header("Breath")]
    [Range(0, 1)] [SerializeField] float startBreath = 0.5f;
    //[SerializeField] AnimationCurve breathCurve;

    [Header("Freeze")]
    [Range(0, 1)] [SerializeField] float startFreeze1 = 0.5f;
    [Range(0, 1)] [SerializeField] float startFreeze2 = 0.3f;
    [Range(0, 1)] [SerializeField] float startBlueOverlay = 0.4f;
    [Range(0, 1)] [SerializeField] float blueColorMaxAlpha = 0.3f;

    [Header("Blood")]
    [Range(0, 10)] [SerializeField] int startBloodVignette = 2;
    [Range(0, 10)] [SerializeField] int startBlood1 = 1;
    [Range(0, 10)] [SerializeField] int startBlood2 = 1;
    [Range(0, 10)] [SerializeField] int startBloodVein = 2;

    [Range(0, 1)] [SerializeField] float bloodHitStep = 0.1f;
    [Range(0, 1)] [SerializeField] float bloodHealStep = 0.1f;

    [Header("Set up")]
    [SerializeField] Image staminaOverlay;
    [SerializeField] Image bloodVignette;
    [SerializeField] Image bloodOverlay1;
    [SerializeField] Image bloodOverlay2;
    [SerializeField] Image bloodVein;
    [SerializeField] Image freeze1Overlay;
    [SerializeField] Image freeze2Overlay;
    [SerializeField] Image blueOverlay;
    //[SerializeField] Image breathOverlay;
    [SerializeField] private VisualEffect breathEffect;


    private GenericHealth playerHealth;
    //private PlayerController controller;
    private bool coldBreath = false;
    //private float breathTimer;
    //private int breathDiv = 6;
 

    private void Start() {
        ResetOverlays();
        playerHealth = GetComponent<GenericHealth>();
        //controller = GetComponent<PlayerController>();
    }

    private void Update() {
        //ColdBreath();
        //if (Keyboard.current.kKey.wasPressedThisFrame)
        //    playerHealth.Hit(1);
        //if (Keyboard.current.jKey.wasPressedThisFrame)
        //    playerHealth.Heal(1);
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
        //breathOverlay.SetAlpha(0);
    }

    //public void ColdBreath() {
    //    float breathSpeed = controller.CurrentSpeed / breathDiv;

    //    breathTimer += Time.deltaTime * breathSpeed;
    //    if (breathTimer > breathCurve.keys[breathCurve.keys.Length - 1].time)
    //        breathTimer = 0;
    //    if (coldBreath) {
    //        breathOverlay.SetAlpha(breathCurve.Evaluate(breathTimer));
    //    } else
    //        breathOverlay.SetAlpha(0);
    //}

    public void SetStamina(float staminaRatio) {
        staminaOverlay.SetAlpha(staminaCurve.Evaluate(staminaRatio));
    }

    /// <summary>
    /// Update blood overlays alpha, called on player hit
    /// </summary>
    /// <param name="tag"></param>
    /// <param name="value"></param>
    public void SetBloodOverlays(string tag, int value) {
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
        if (temperatureRatio < startBreath) {
            breathEffect.enabled = true;
        } else {
            breathEffect.enabled = false;
        }
        CalculateFreezeAlpha(ref freeze1Overlay, temperatureRatio, startFreeze1);
        CalculateFreezeAlpha(ref freeze2Overlay, temperatureRatio, startFreeze2);

        if (temperatureRatio < startBlueOverlay) {
            float factor = blueColorMaxAlpha / startBlueOverlay;
            blueOverlay.SetAlpha(blueColorMaxAlpha - factor * temperatureRatio);
        }
        else
            blueOverlay.SetAlpha(0);
    }

    public void CalculateFreezeAlpha(ref Image img, float ratio, float startThreshold) {
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
