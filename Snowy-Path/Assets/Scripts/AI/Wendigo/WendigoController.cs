using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WendigoController : MonoBehaviour {

    private NavMeshAgent m_agent;
    private Transform m_player;

    void Start() {
        m_player = FindObjectOfType<PlayerController>().transform;
        m_agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {
        m_agent.SetDestination(m_player.position);
    }
}
