using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfController : MonoBehaviour {

    public GameObject target;
    private NavMeshAgent navMeshAgent;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update() {
        HeadForDestination();
    }

    private void HeadForDestination() {
        Vector3 destination = target.transform.position;
        navMeshAgent.SetDestination(destination);
    }
}
