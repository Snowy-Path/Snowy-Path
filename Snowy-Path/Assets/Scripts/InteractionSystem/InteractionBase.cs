using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class for any interaction script.
/// Provides abstract methods for derived classes in order to be interactable.
/// </summary>
[DisallowMultipleComponent]
public abstract class InteractionBase : MonoBehaviour {

    /// <summary>
    /// Do the interaction.
    /// </summary>
    public abstract void Interact();

    /// <summary>
    /// Show the interaction feedback.
    /// Cound be any feedback (UI, VFX, SFX ...)
    /// </summary>
    public abstract void ShowInteractionFeedback();

    /// <summary>
    /// Hide the interaction feedback.
    /// </summary>
    public abstract void HideInteractionFeedback();
}