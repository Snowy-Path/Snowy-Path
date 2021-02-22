using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This script makes the gameobject interactable.
/// Provides events called from the InteractionController script.
/// </summary>
[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour {

    #region Variables

    [Header("Deactivation settings")]

    [SerializeField]
    [Tooltip("If false, the interaction and feedbacks doesn't occur.")]
    private bool isActive = true;
    public bool IsActive {
        get => isActive;
        set {
            if (isActive == value) {
                return;
            }
            isActive = value;
            if (isActive) {
                onShowFeedback.Invoke();
            } else {
                onHideFeedback.Invoke();
            }
        }
    }


    [Header("UnityEvents callbacks")]

    [Tooltip("Callbacks called when the player hits the interact input.")]
    public UnityEvent onInteract;

    [Tooltip("Callbacks called when the player's camera looks at the object")]
    public UnityEvent onShowFeedback;

    [Tooltip("Callbacks called when the player's camera doesn't look anymore at the object")]
    public UnityEvent onHideFeedback;

    #endregion


    #region Interaction Methods

    /// <summary>
    /// Triggers the UnityEvent onInteract.
    /// Used when the player interact with the gameobject.
    /// </summary>
    public void Interact() {
        if (IsActive) {
            onInteract.Invoke();
        }
    }

    /// <summary>
    /// Triggers the show interaction feedback.
    /// Could be any feedback (UI, VFX, SFX ...)
    /// Used when the player looks at the gameobject.
    /// MUST NOT BE USED ITSELF ON THE onShowFeedback UnityEvent.
    /// </summary>
    public void ShowInteractionFeedback() {
        if (IsActive) {
            onShowFeedback.Invoke();
        }
    }

    /// <summary>
    /// Triggers the hide interaction feedback.
    /// Used when the player stops looking at the gameobject.
    /// MUST NOT BE USED ITSELF ON THE onHideFeedback UnityEvent.
    /// </summary>
    public void HideInteractionFeedback() {
        if (IsActive) {
            onHideFeedback.Invoke();
        }
    }

    #endregion


    #region Utility Method

    /// <summary>
    /// Destroy itself.
    /// </summary>
    public void DestroyItself() {
        Destroy(gameObject);
    }

    /// <summary>
    /// Switch activation state at each call.
    /// </summary>
    public void SwitchActivation() {
        IsActive = !IsActive;
    }

    /// <summary>
    /// Show the outline feedback from the shader.
    /// </summary>
    public void ShowOutlineEffect() {
        GetComponent<Renderer>().materials[0].SetFloat("IsActive", 1);
    }

    /// <summary>
    /// Hide the outline feedback from the shader.
    /// </summary>
    public void HideOutlineEffect() {
        GetComponent<Renderer>().materials[0].SetFloat("IsActive", 0);
    }

    #endregion

    private void OnDestroy() {
        for (int i = 0; i < GetComponent<Renderer>().materials.Length; i++) {
            Destroy(GetComponent<Renderer>().materials[i]);
        }
    }

}