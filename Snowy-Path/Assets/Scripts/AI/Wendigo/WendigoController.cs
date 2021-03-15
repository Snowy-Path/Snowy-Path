using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WendigoController : MonoBehaviour, IEnnemyController {

    private NavMeshAgent m_agent;
    private Transform m_player;

    private float m_normalSpeed;
    public float slowSpeed = 3.75f;
    public float slowDuration = 5f;

    void Awake() {
        m_player = FindObjectOfType<PlayerController>().transform;
        m_agent = GetComponent<NavMeshAgent>();
        m_normalSpeed = m_agent.speed;
    }

    // Update is called once per frame
    void Update() {
        m_agent.SetDestination(m_player.position);
    }

    IEnumerator ReduceSpeed() {
        m_agent.speed = slowSpeed;
        yield return new WaitForSeconds(slowDuration);
        m_agent.speed = m_normalSpeed;
    }

    #region IEnnemyController
    public void Hit(EToolType toolType, int attackDamage) {
        if (toolType == EToolType.Pistol) { // If Gun
            StartCoroutine(ReduceSpeed());
        }
    }
    #endregion
}
