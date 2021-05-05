﻿using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Controls the interaction input and trigger interaction if it is possible.
/// The detection depends on a layer.
/// </summary>
public class InteractionController : MonoBehaviour {

    #region Variables

    [Header("Interaction settings")]

    [SerializeField]
    [Min(0.0f)]
    [Tooltip("The maximum distance to interact with any interactable object.")]
    private float maxDistance = 2.0f;

    [SerializeField]
    [Min(0.0f)]
    [Tooltip("The on-screen radius of detection. Allow player to interact with objects than is not exactly on the cursor. The more the value is, the more flexible it is.")]
    private float radius = 0.05f;

    [SerializeField]
    [Tooltip("The only layer the detection use.")]
    private LayerMask interactableLayer;

    [SerializeField]
    [Tooltip("Layers blocking the interaction.")]
    private LayerMask blockingLayers;

    [SerializeField]
    [Header("Set up")]
    private Camera playerCamera;

    [SerializeField] Animator animator;

    private Interactable m_interactable;

    #endregion

    #region Built In Methods

    /// <summary>
    /// Called at each frame. Checks if an Interactable asset is detected.
    /// </summary>
    void Update() {
        CheckForInteractable();
        animator.SetBool("Hover", CanInteract());
    }

    #endregion

    #region Methods

    /// <summary>
    /// Checks if an interactable is detected through a raycast in the camera direction. If it is, retrieves the detected object interface.
    /// </summary>
    private void CheckForInteractable() {

        Ray _rayInteract = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit _hitInfoInteract;

        bool _hitInteract = Physics.SphereCast(_rayInteract, radius, out _hitInfoInteract, maxDistance, interactableLayer);

        // If we hit an object
        if (_hitInteract) {
            Interactable _interHit = _hitInfoInteract.transform.GetComponent<Interactable>(); // Try to retrieve it's interactable component

            if (_interHit != null) { // We really did hit an interactable object

                // Getting the point to raycast to
                // Allow us to ensure we can interact with the Interactable found with the first ray
                Vector3 _interPosition;
                if (_interHit.InteractionPoint) {
                    _interPosition = _interHit.InteractionPoint.position;
                }
                else {
                    _interPosition = _interHit.transform.position;
                }

                Ray _rayBlock = new Ray(playerCamera.transform.position, _interPosition - playerCamera.transform.position);
                RaycastHit _hitInfoBlock;

                if (Physics.Raycast(_rayBlock, out _hitInfoBlock, maxDistance, blockingLayers)) {
                    if (_hitInfoBlock.transform.GetInstanceID() == _hitInfoInteract.transform.GetInstanceID()) {
                        // Can interact with it
                        if (!_interHit.IsActive) {
                            if (m_interactable != null) { // Hide previous object if it was a real object
                                m_interactable.HideInteractionFeedback();
                                m_interactable = null;
                            }
                        }
                        else if (m_interactable != _interHit) { //If previous object is different from current object

                            if (m_interactable != null) { // Hide previous object if it was a real object
                                m_interactable.HideInteractionFeedback();
                            }

                            // Switch and show current object if it is active
                            m_interactable = _interHit;
                            m_interactable.ShowInteractionFeedback();
                        }
                    }
                    else {
                        // Hide previous object if it was a real object & switch to null
                        if (m_interactable != null) {
                            m_interactable.HideInteractionFeedback();
                            m_interactable = null;
                        }
                    }
                }

            }
            else { // A non-interactable object was detected

                // Hide previous object if it was a real object & switch to null
                if (m_interactable != null) {
                    m_interactable.HideInteractionFeedback();
                    m_interactable = null;
                }
            }

        }
        else if (m_interactable != null) {
            // If nothing was hit
            // Hide previous object if it was a real object & switch to null
            m_interactable.HideInteractionFeedback();
            m_interactable = null;
        }

        Debug.DrawRay(_rayInteract.origin, _rayInteract.direction * maxDistance, _hitInteract ? Color.green : Color.red);

    }

    internal bool CanInteract() {
        return m_interactable != null && m_interactable.IsActive;
    }

    /// <summary>
    /// Check if there is an available Interactable object and if it is possible to interact with it.
    /// If so, makes the interaction.
    /// </summary>
    private void CheckForInteractableInput() {
        if (CanInteract()) {
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
