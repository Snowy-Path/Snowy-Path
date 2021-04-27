﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Represent the Wendigo Artificial Intelligence.
/// </summary>
public class WendigoController : MonoBehaviour, IEnnemyController {

    #region Variables
    private NavMeshAgent m_agent;
    private Transform m_player; //Player's root transform

    private float m_normalSpeed; //Normal speed retrieved from NavMeshAgent

    [Tooltip("Wendigo speed when hit by gun.")]
    [Min(0)]
    [SerializeField]
    private float slowSpeed = 3.75f;

    [SerializeField]
    [Tooltip("Effect animator.")]
    private Animator m_effectAnimator;

    [Tooltip("Wendigo slowed donw duration when hit by gun.")]
    [Min(0)]
    [SerializeField]
    private float slowDuration = 5f;

    #region SFX
    [Header("SFX")]
    [SerializeField]
    [FMODUnity.EventRef]
    private string m_spawnEventPath;

    [SerializeField]
    [FMODUnity.EventRef]
    private string m_damagedEventPath;

    [SerializeField]
    private GameObject m_SFXGameObject;
    #endregion
    #endregion


    #region Built-In Methods
    /// <summary>
    /// Call at gameobject creation.
    /// Find and fill fields.
    /// </summary>
    void Start() {
        m_player = FindObjectOfType<PlayerController>().transform;
        m_agent = GetComponent<NavMeshAgent>();
        m_normalSpeed = m_agent.speed;
    }

    internal void Reset() {
        m_agent.speed = m_normalSpeed;
    }

    /// <summary>
    /// Call at each frame update.
    /// Set destination to the player.
    /// </summary>
    void Update() {
        m_agent.SetDestination(m_player.position);
    }
    #endregion

    #region Coroutines
    /// <summary>
    /// Coroutine reducing Wendigo speed for <c>slowDuration</c> seconds.
    /// </summary>
    IEnumerator ReduceSpeed() {
        m_agent.speed = slowSpeed;
        yield return new WaitForSeconds(slowDuration);
        m_agent.speed = m_normalSpeed;
    }
    #endregion

    #region IEnnemyController
    /// <summary>
    /// If hit by a pistol, reduce speed for multiple seconds.
    /// </summary>
    /// <param name="toolType">The type of tool that called this method. Used to differentiate between Pistol and Torch weapons.</param>
    /// <param name="attackDamage">The damage value to be dealt.</param>
    public void Hit(EToolType toolType, int attackDamage) {
        if (toolType == EToolType.Pistol) { // If Gun
            StartCoroutine(ReduceSpeed());
            m_effectAnimator.SetTrigger("TookDamage");
        }
    }
    #endregion

    #region Utility
    /// <summary>
    /// Teleports the Wendigo using the NavMeshAgent to the nearest point of <c>newWendigoPos</c> on the NavMesh.
    /// </summary>
    /// <param name="newWendigoPos">The new position to warp at.</param>
    internal void Teleport(Transform newWendigoPos) {
        m_agent.Warp(newWendigoPos.position);
    }
    #endregion

    #region SFX_Utility
    private void PlaySpawnSFX() {
        FMODUnity.RuntimeManager.PlayOneShotAttached(m_spawnEventPath, m_SFXGameObject);
    }
    internal void PlayDamagedSFX() {
        FMODUnity.RuntimeManager.PlayOneShotAttached(m_damagedEventPath, m_SFXGameObject);
    }
    #endregion
}
