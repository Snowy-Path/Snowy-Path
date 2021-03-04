using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfController : MonoBehaviour {

    #region Variables

    #region Idle
    [Min(0)]
    public float waitingTime;
    internal float waitingTimer;
    #endregion

    #region MoveToWaypoint
    public List<Transform> waypoints;
    private int waypointIndex = 0;
    #endregion

    #region Seeing sense
    public bool isSeeingPlayer = false;
    public Transform target;
    #endregion

    private StateMachine m_fsm;
    internal NavMeshAgent agent;

    #endregion

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        HFSMInitialization();
    }


    private void Update() {

#if DEBUG
        Debug.DrawLine(transform.position, agent.destination, Color.yellow);
        Vector3[] corners = agent.path.corners;
        for (int i = 1; i < corners.Length; i++) {
            Debug.DrawLine(corners[i - 1], corners[i], new Color(0.5f, 0, 1));
        }
#endif

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

    #region HFSM
    private void HFSMInitialization() {
        m_fsm = new StateMachine(EStateType.None);

        Patrol_Init(m_fsm);
        Combat_Init(m_fsm);

        m_fsm.defaultState = EStateType.Patrol;
        m_fsm.OnEntry();
    }

    #region Patrol

    private void Patrol_Init(StateMachine parent) {
        StateMachine patrol = new StateMachine(EStateType.Patrol, parent);

        patrol.AddTransition(new Transition(
            EStateType.Combat,
            (condition) => isSeeingPlayer
        ));

        Idle_Init(patrol);
        MoveToWaypoint_Init(patrol);

        patrol.defaultState = EStateType.MoveToWaypoint;
        parent.AddState(patrol);
    }

    private void Idle_Init(StateMachine parent) {
        State idle = new State(EStateType.Idle, parent,
            onEntry: (state) => waitingTimer = 0f,
            onExit: (state) => {
                waitingTimer = 0f;
                waypointIndex++;
                if (waypointIndex >= waypoints.Count) {
                    waypointIndex = 0;
                }
            },
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
                agent.SetDestination(waypoints[waypointIndex].position); // To set a destination before transitions are checked
            },
            onExit: (state) => {
                if (agent.hasPath) {
                    agent.ResetPath();
                }
            }
        );

        moveToWaypoint.AddTransition(new Transition(
            EStateType.Idle,
            (condition) => agent.remainingDistance < 0.5f
        ));

        parent.AddState(moveToWaypoint);
    }
    #endregion

    #region Combat
    private void Combat_Init(StateMachine parent) {
        StateMachine combat = new StateMachine(EStateType.Combat, parent);

        combat.AddTransition(new Transition(
            EStateType.Patrol,
            (condition) => !isSeeingPlayer
        ));

        Attack_Init(combat);

        combat.defaultState = EStateType.Aggro;
        parent.AddState(combat);
    }

    private void Attack_Init(StateMachine parent) {
        State attack = new State(EStateType.Aggro, parent,
            onUpdate: (state) => {
                agent.SetDestination(target.position);
            }
        );
        parent.AddState(attack);
    }
    #endregion

    #endregion
}
