using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfController : MonoBehaviour {

    #region Inspector variables
    [Min(0)]
    public float waitingTime;
    internal float waitingTimer;
    #endregion

    private StateMachine m_fsm;
    private NavMeshAgent navMeshAgent;

    private void Start() {
        navMeshAgent = GetComponent<NavMeshAgent>();

        m_fsm = new StateMachine(this);

        StateBase idle = new Idle(this);
        idle.AddTransition(
            new Transition(
                EStateType.MoveToWaypoint,
                (condition) => waitingTimer >= waitingTime
            )
        );

        StateBase moveToWaypoint = new MoveToWaypoint(this);
        moveToWaypoint.AddTransition(
            new Transition(
                EStateType.Idle,
                (condition) => HasReachedWaypoint()
            )
        );


        m_fsm.AddState(idle);
        m_fsm.AddState(moveToWaypoint);

        m_fsm.OnEntry();
    }

    private bool HasReachedWaypoint() {
        return false;
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
}
