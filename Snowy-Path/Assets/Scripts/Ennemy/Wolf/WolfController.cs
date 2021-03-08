using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfController : MonoBehaviour {

    #region Variables

    #region Patrol
    public float loosingTime;
    #endregion

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
    private Transform target;
    private bool isSeeingPlayer = false;
    public Transform Target {
        get {
            return target;
        }
        set {
            target = value;
            if (target) {
                lastSaw = Time.time;
                isSeeingPlayer = true;
            } else {
                isSeeingPlayer = false;
            }
        }
    }
    private float lastSaw = 0f;
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
        Debug.DrawLine(transform.position, agent.destination, Color.magenta);
        Vector3[] corners = agent.path.corners;
        for (int i = 1; i < corners.Length; i++) {
            Debug.DrawLine(corners[i - 1], corners[i], Color.yellow);
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
        m_fsm = new StateMachine(EStateType.None, EStateType.Patrol);

        Patrol_Init(m_fsm);
        Combat_Init(m_fsm);

        m_fsm.OnEntry();
    }


    #region Patrol
    private void Patrol_Init(StateMachine parent) {
        StateMachine patrol = new StateMachine(EStateType.Patrol, EStateType.MoveToWaypoint, parent);

        patrol.AddTransition(new Transition(
            EStateType.Combat,
            (condition) => isSeeingPlayer
        ));

        Idle_Init(patrol);
        MoveToWaypoint_Init(patrol);

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
        StateMachine combat = new StateMachine(EStateType.Combat, EStateType.Aggro, parent);

        combat.AddTransition(new Transition(
            EStateType.Patrol,
            (condition) => HasLostPlayer()
        ));

        Attack_Init(combat);
        //TODO: Add states

        parent.AddState(combat);
    }

    private bool HasLostPlayer() {
        return (Time.time - lastSaw) >= loosingTime;
    }

    private void Attack_Init(StateMachine parent) {
        State attack = new State(EStateType.Aggro, parent,
            onUpdate: (state) => {
                if (target) {
                    agent.SetDestination(target.position);
                }
            }
        );
        parent.AddState(attack);
    }
    #endregion


    #endregion
}
