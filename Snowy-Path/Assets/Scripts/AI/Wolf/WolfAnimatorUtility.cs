using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manage animation events and the rotation of the model depending on the ground orientation.
/// </summary>
[RequireComponent(typeof(Animator))]
public class WolfAnimatorUtility : MonoBehaviour {

    #region Variables
    [SerializeField]
    private WolfController m_wolfController;

    [SerializeField]
    private LayerMask m_terrainMask;

    private RaycastHit m_hit;
    private Vector3 m_theRay;
    #endregion

    /// <summary>
    /// Animation Event throw at the end of the Inspect animation.
    /// Makes the WolfController transition outside the Inspect state.
    /// </summary>
    public void InspectAnimationFinished() {
        m_wolfController.InspectAnimationFinished = true;
    }

    /// <summary>
    /// Animation Event throw at the end of the Death animation.
    /// Destroy the entire Wolf;
    /// </summary>
    public void Dead() {
        m_wolfController.DestroyItself();
    }

    /// <summary>
    /// Called at each frame to rotate the model depending on ground orientation.
    /// </summary>
    private void Update() {
        Align();
    }

    /// <summary>
    /// Rotate the model depending on ground orientation.
    /// </summary>
    private void Align() {
        m_theRay = -transform.up;

        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), m_theRay, out m_hit, 20, m_terrainMask)) {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, m_hit.normal) * transform.parent.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime / 0.15f);
        }
    }

    public void MovementSoundEmission() {
        m_wolfController.MovementSoundEmission();
    }

}
