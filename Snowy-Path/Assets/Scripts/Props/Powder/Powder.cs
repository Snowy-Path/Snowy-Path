using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Holds the interaction method when interacting with the powder prefab.
/// </summary>
public class Powder : MonoBehaviour {

    private PowderController powderController;

    /// <summary>
    /// Retrieves the PowderController in the scene.
    /// </summary>
    private void Start() {
        powderController = FindObjectOfType<PowderController>();
    }

    /// <summary>
    /// Call the PowderController in order to activate the effect.
    /// </summary>
    public void ActivatePowderEffects() {
        if (powderController != null) {
            powderController.ActivatePowderEffects();
        }
        var playerPlayable = FindObjectOfType<PlayerPlayable>();
        if(playerPlayable != null)
        {
            playerPlayable.playablePickUpPowder.stopped += OnPlayableDirectorStopped;
            playerPlayable.playablePickUpPowder.Play();
        }

    }

    public void PlaySFX() {
        FMODUnity.StudioEventEmitter m_emitter;
        m_emitter = GameObject.Find("SFXPowder").GetComponent<FMODUnity.StudioEventEmitter>();
        m_emitter.Play();
    }

    void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        aDirector.stopped -= OnPlayableDirectorStopped;
        gameObject.GetComponent<TeleportPlayer>()?.Teleport();
    }

}
