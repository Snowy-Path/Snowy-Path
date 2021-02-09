using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controls the interaction input and trigger interaction if it is possible
/// </summary>
public class InteractionController : MonoBehaviour {

    [Header("Interaction settings")]

    #region variables
    public float interactionMaxDistance;
    public LayerMask interactableLayer;
    public Camera playerCamera;
    private InteractionBase m_interactable;
    #endregion

    /// <summary>
    /// Called at each frame. Checks if a IInteractable asset is detected and interact with it.
    /// </summary>
    void Update() {
        CheckForInteractable();
        CheckForInteractableInput();
    }

    /// <summary>
    /// Checks if an interactable is detected through a raycast in the camera direction. If it is, retrieves the detected object interface
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
    /// 
    /// </summary>
    private void CheckForInteractableInput() {
        if (Input.GetKeyDown(KeyCode.C)) {
            if (m_interactable != null) {
                m_interactable.Interact();
            }
        }
    }
}
