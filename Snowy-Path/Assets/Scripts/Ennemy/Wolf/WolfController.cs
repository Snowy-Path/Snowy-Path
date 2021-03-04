using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfController : MonoBehaviour {

    #region Idle variables
    [Min(0)]
    public float waitingTime;
    internal float waitingTimer;
    #endregion

    #region MoveToWaypoint variables
    public List<Transform> waypoints;
    private int waypointIndex = -1;
    #endregion

    private StateMachine m_fsm;
    internal NavMeshAgent agent;

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        HFSMInitialization();
    }


    private void Update() {
        m_fsm.OnUpdate();
    }

    private void FixedUpdate() {
        m_fsm.OnFixedUpdate();
    }

    private void LateUpdate() {
        m_fsm.OnLateUpdate();
    }

    private void OnDestroy() {
        m_fsm.OnExit();
    }



    #region HFSM Initialisation
    private void HFSMInitialization() {
        m_fsm = new StateMachine(EStateType.None);

        Patrol_Init(m_fsm);

        m_fsm.defaultState = EStateType.Patrol;
        m_fsm.OnEntry();
    }

    private void Patrol_Init(StateMachine parent) {

        StateMachine patrol = new StateMachine(EStateType.Patrol, parent);
        parent.AddState(patrol);

        Idle_Init(patrol);
        MoveToWaypoint_Init(patrol);

        patrol.defaultState = EStateType.MoveToWaypoint;
        patrol.OnEntry();
    
    }

    private void Idle_Init(StateMachine parent) {
        State idle = new State(EStateType.Idle, parent,
            onEntry: (state) => waitingTimer = 0f,
            onExit: (state) => waitingTimer = 0f,
            onUpdate: (state) => waitingTimer += Time.deltaTime
        );

        idle.AddTransition(new Transition(
            EStateType.MoveToWaypoint,
            (condition) => waitingTimer >= waitingTime
        ));

        parent.AddState(idle);
    }

    private void MoveToWaypoint_Init(StateMachine parent) {
        State moveToWaypoint = new State(EStateType.MoveToWaypoint, parent,

            onEntry: (state) => {
                waypointIndex++;
                if (waypointIndex >= waypoints.Count) {
                    waypointIndex = 0;
                }
                agent.SetDestination(waypoints[waypointIndex].position); // To set a destination before transitions are checked
            },

            onUpdate: (state) => agent.SetDestination(waypoints[waypointIndex].position)
        );

        moveToWaypoint.AddTransition(new Transition(
            EStateType.Idle,
            (condition) => agent.remainingDistance < 0.5f
        ));

        parent.AddState(moveToWaypoint);
    }
    #endregion
}
