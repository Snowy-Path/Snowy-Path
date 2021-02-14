using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Combined with the Interactable layer, this script makes the gameobject interactable.
/// Provides events called from the InteractionController script.
/// </summary>
[DisallowMultipleComponent]
public class Interactable : MonoBehaviour {

    #region Variables

    [Header("Deactivation settings")]

    [SerializeField]
    [Tooltip("If false, the interaction and feedbacks doesn't occur.")]
    private bool isActive = true;
    public bool IsActive { get => isActive; }


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
        onInteract.Invoke();
    }

    /// <summary>
    /// Triggers the show interaction feedback.
    /// Could be any feedback (UI, VFX, SFX ...)
    /// Used when the player looks at the gameobject.
    /// </summary>
    public void ShowInteractionFeedback() {
        onShowFeedback.Invoke();
    }

    /// <summary>
    /// Triggers the hide interaction feedback.
    /// Used when the player stops looking at the gameobject.
    /// </summary>
    public void HideInteractionFeedback() {
        onHideFeedback.Invoke();
    }

    #endregion


    #region Utility Method

    /// <summary>
    /// Destroy itself
    /// </summary>
    public void DestroyItself() {
        Destroy(gameObject);
    }

    /// <summary>
    /// Change the activation state of the current interaction script.
    /// If it was true, it is now set to false and vice-versa.
    /// </summary>
    public void SwitchActivation() {
        isActive = !isActive;
    }

    #endregion

}