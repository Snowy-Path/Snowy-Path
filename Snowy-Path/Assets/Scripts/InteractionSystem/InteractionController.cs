using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the interaction input and trigger interaction if it is possible
/// </summary>
public class InteractionController : MonoBehaviour {

    #region Variables

    [Header("Interaction settings")]

    public float interactionMaxDistance;
    public LayerMask interactableLayer;
    public Camera playerCamera;
    private InteractionBase m_interactable;

    #endregion

    #region Built In Methods

    /// <summary>
    /// Called at each frame. Checks if a IInteractable asset is detected and interact with it.
    /// </summary>
    void Update() {
        CheckForInteractable();
    }

    #endregion

    #region Methods

    /// <summary>
    /// Checks if an interactable is detected through a raycast in the camera direction. If it is, retrieves the detected object interface.
    /// </summary>
    private void CheckForInteractable() {

        Ray _ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit _hitInfo;

        bool _hitSomething = Physics.Raycast(_ray, out _hitInfo, interactionMaxDistance, interactableLayer);

        if (_hitSomething) {
            InteractionBase _interactable = _hitInfo.transform.GetComponent<InteractionBase>();

            if (_interactable != null) { // We really did hit an interactable object

                if (m_interactable != _interactable) {
                    m_interactable = _interactable;
                }

            } else {
                m_interactable = null;
            }
        }

        Debug.DrawRay(_ray.origin, _ray.direction * interactionMaxDistance, _hitSomething ? Color.green : Color.red);

    }


    /// <summary>
    /// Check if the player pressed the interaction button and call the interact method if so.
    /// </summary>
    private void CheckForInteractableInput() {
        if (m_interactable != null) {
            m_interactable.Interact();
        }
    }

    /// <summary>
    /// Method called in the Unity Event "Interact" from the Input System.
    /// Call the utility method which interact with the detected object.
    /// </summary>
    public void OnInteract(InputAction.CallbackContext context) {
        if (context.phase.Equals(InputActionPhase.Performed)) {
            CheckForInteractableInput();
        }
    }

    #endregion

}
