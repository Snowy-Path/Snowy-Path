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

    [Tooltip("Wendigo slowed donw duration when hit by gun.")]
    [Min(0)]
    [SerializeField]
    private float slowDuration = 5f;
    #endregion


    #region Built-In Methods
    /// <summary>
    /// Call at gameobject creation.
    /// Find and fill fields.
    /// </summary>
    void Awake() {
        m_player = FindObjectOfType<PlayerController>().transform;
        m_agent = GetComponent<NavMeshAgent>();
        m_normalSpeed = m_agent.speed;
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
}
