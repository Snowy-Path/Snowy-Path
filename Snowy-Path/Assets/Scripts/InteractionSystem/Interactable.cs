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

    public UnityEvent onInteract;
    public UnityEvent onShowFeedback;
    public UnityEvent onHideFeedback;

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
}