using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfController : MonoBehaviour {

    #region Variables

    public Animator animator;

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
    private float lastSaw = float.NegativeInfinity;
    #endregion

    #region Hearing sense
    private Vector3 soundPosition;
    public Vector3 SoundPosition {
        get {
            return soundPosition;
        }
        set {
            soundPosition = value;
            lastHeard = Time.time;
        }
    }
    private float lastHeard = float.NegativeInfinity;
    public float hearingThreshold;
    #endregion

    #region Aggro
    
    #endregion


    private StateMachine m_fsm;
    internal NavMeshAgent agent;

    #endregion

    #region Built-In Methods

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
    #endregion

    #region HFSM
    private void HFSMInitialization() {
        m_fsm = new StateMachine(EStateType.None, EStateType.Patrol);

        Patrol_Init(m_fsm);
        Inspecting_Init(m_fsm);
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

        patrol.AddTransition(new Transition(
            EStateType.Inspecting,
            (condition) => (Time.time - lastHeard) < hearingThreshold
        ));

        Idle_Init(patrol);
        MoveToWaypoint_Init(patrol);

        parent.AddState(patrol);
    }

    private void Idle_Init(StateMachine parent) {
        State idle = new State(EStateType.Idle, parent,
            onEntry: (state) => waitingTimer = Time.time + waitingTime,
            onExit: (state) => {
                waypointIndex++;
                if (waypointIndex >= waypoints.Count) {
                    waypointIndex = 0;
                }
            }
        );

        idle.AddTransition(new Transition(
            EStateType.MoveToWaypoint,
            (condition) => Time.time >= waitingTimer
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

    #region
    private void Inspecting_Init(StateMachine parent) {
        State inspecting = new State(EStateType.Inspecting, parent,
            onEntry: (state) => {
                agent.SetDestination(SoundPosition);
            }    
        );

        inspecting.AddTransition(new Transition(
            EStateType.Combat,
            (condition) => isSeeingPlayer
        ));

        inspecting.AddTransition(new Transition(
            EStateType.Patrol,
            (condition) => (Time.time - lastHeard) >= hearingThreshold
        ));

        parent.AddState(inspecting);
    }
    #endregion

    #region Combat
    private void Combat_Init(StateMachine parent) {
        StateMachine combat = new StateMachine(EStateType.Combat, EStateType.Aggro, parent);

        combat.AddTransition(new Transition(
            EStateType.Patrol,
            (condition) => (Time.time - lastSaw) >= loosingTime
        ));

        Aggro_Init(combat);
        Lurking_Init(combat);
        Charge_Init(combat);
        Attack_Init(combat);
        Recovery_Init(combat);
        Escaping_Init(combat);
        Stun_Init(combat);

        parent.AddState(combat);
    }

    private void Aggro_Init(StateMachine parent) {
        State aggro = new State(EStateType.Aggro, parent,
            onEntry: (state) => {
                animator.SetTrigger("Aggro");
            }
        );

        aggro.AddTransition(new Transition(
            EStateType.Lurking,
            (condition) => !animator.GetCurrentAnimatorStateInfo(0).IsName("Aggro")
        ));

        parent.AddState(aggro);
    }

    private void Lurking_Init(StateMachine parent) {
        State lurking = new State(EStateType.Lurking, parent
        );
        parent.AddState(lurking);
    }

    private void Charge_Init(StateMachine parent) {
        State charge = new State(EStateType.Charge, parent
        );
        parent.AddState(charge);
    }

    private void Attack_Init(StateMachine parent) {
        State attack = new State(EStateType.Attack, parent
        );
        parent.AddState(attack);
    }

    private void Recovery_Init(StateMachine parent) {
        State recovery = new State(EStateType.Recovery, parent
        );
        parent.AddState(recovery);
    }

    private void Escaping_Init(StateMachine parent) {
        State escaping = new State(EStateType.Escaping, parent
        );
        parent.AddState(escaping);
    }

    private void Stun_Init(StateMachine parent) {
        State stun = new State(EStateType.Stun, parent
        );
        parent.AddState(stun);
    }
    #endregion


    #endregion

    public EStateType GetCurrentState() {
        return m_fsm.GetCurrentState();
    }
}
