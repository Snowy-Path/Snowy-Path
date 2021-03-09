using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfController : MonoBehaviour {

    #region Variables

    public Animator animator;
    private float timer;

    #region Patrol
    public float loosingTime;
    #endregion

    #region Idle
    [Min(0)]
    public float idleWaitingTime = 5f;
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
    private bool aggroFinished = false;
    public void AggroAnimationFinished() {
        aggroFinished = true;
    }
    #endregion

    #region Lurking
    public float lurkingDistance = 10f;
    public float lurkingTime = 5f;
    #endregion

    #region Charge
    public float chargeSpeed;
    private float normalSpeed;
    //private Vector3 chargePosition;
    #endregion

    #region Attack
    private bool attackFinished = false;
    public void AttackAnimationFinished() {
        attackFinished = true;
    }
    #endregion

    #region Recovery
    public float recoveryTime = 5f;
    private int recoveryHealth = int.MinValue;
    #endregion

    #region Escaping

    #endregion

    #region Stun

    #endregion


    private StateMachine m_fsm;
    internal NavMeshAgent agent;
    private GenericHealth m_genericHealth;

    #endregion

    #region Built-In Methods

    private void Start() {
        agent = GetComponent<NavMeshAgent>();
        m_genericHealth = GetComponent<GenericHealth>();
        HFSMInitialization();
        normalSpeed = agent.speed;
    }
    private void Update() {

#if DEBUG
        Debug.DrawLine(transform.position, agent.destination, Color.magenta);
        Vector3[] corners = agent.path.corners;
        for (int i = 1; i < corners.Length; i++) {
            Debug.DrawLine(corners[i - 1], corners[i], Color.yellow);
        }
        //Debug.Log(GetCurrentState());
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
            onEntry: (state) => timer = Time.time + idleWaitingTime,
            onExit: (state) => {
                waypointIndex++;
                if (waypointIndex >= waypoints.Count) {
                    waypointIndex = 0;
                }
                timer = float.NegativeInfinity;
            }
        );

        idle.AddTransition(new Transition(
            EStateType.MoveToWaypoint,
            (condition) => Time.time >= timer
        ));

        parent.AddState(idle);
    }

    private void MoveToWaypoint_Init(StateMachine parent) {
        State moveToWaypoint = new State(EStateType.MoveToWaypoint, parent,
            onEntry: (state) => {
                agent.SetDestination(waypoints[waypointIndex].position); // To set a destination before transitions are checked
            },
            //onUpdate: (state) => { // Can be removed for performance purposes
            //    agent.SetDestination(waypoints[waypointIndex].position); // To compute a shorter path
            //},
            onExit: (state) => {
                ResetAgentPath();
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
            //onEntry: (state) => { // To set a destination before transitions are checked
            //    agent.SetDestination(SoundPosition);
            //},
            onUpdate: (state) => { // Can be removed for performance purposes
                agent.SetDestination(SoundPosition);
            },
            onExit: (state) => {
                ResetAgentPath();
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
            },
            onExit: (state) => {
                aggroFinished = false;
            }
        );

        aggro.AddTransition(new Transition(
            EStateType.Lurking,
            (condition) => aggroFinished
        ));

        parent.AddState(aggro);
    }

    private void Lurking_Init(StateMachine parent) {
        State lurking = new State(EStateType.Lurking, parent,
            onEntry: (state) => {
                //agent.stoppingDistance = 10f;
                timer = Time.time + lurkingTime;
            },
            onUpdate: (state) => {
                if (target) {
                    //agent.SetDestination(target.position);
                    //if (Vector3.Distance(transform.position, target.position) < lurkingDistance) {
                    //    //TODO: Move avay
                    //} else {
                    //    //TODO : move in circle around the player
                    //}
                }
            },
            onExit: (state) => {
                //agent.stoppingDistance = 0f;
                timer = float.NegativeInfinity;
            }
        );

        lurking.AddTransition(new Transition(
            EStateType.Charge,
            (condition) => Time.time >= timer
        ));

        parent.AddState(lurking);
    }

    private void Charge_Init(StateMachine parent) {
        State charge = new State(EStateType.Charge, parent,
            onEntry: (state) => {
                agent.speed = chargeSpeed;
                agent.autoBraking = false;
                agent.SetDestination(target.position);
            },
            onUpdate: (state) => {
                agent.SetDestination(target.position);
            },
            onExit: (state) => {
                agent.speed = normalSpeed;
                ResetAgentPath();
            }
        );

        charge.AddTransition(new Transition(
            EStateType.Attack,
            (condition) => agent.remainingDistance < 1f
        ));

        parent.AddState(charge);
    }

    private void Attack_Init(StateMachine parent) {
        State attack = new State(EStateType.Attack, parent,
            onEntry: (state) => {
                animator.SetTrigger("Attack");
            },
            onExit: (state) => {
                attackFinished = false;
            }
        );

        attack.AddTransition(new Transition(
            EStateType.Recovery,
            (condition) => attackFinished
        ));

        parent.AddState(attack);
    }

    private void Recovery_Init(StateMachine parent) {
        State recovery = new State(EStateType.Recovery, parent,
            onEntry: (state) => {
                timer = Time.time + lurkingTime;
                recoveryHealth = m_genericHealth.GetCurrentHealth();
            },
            onExit: (state) => {
                timer = float.NegativeInfinity;
                recoveryHealth = int.MinValue;
            }
        );

        recovery.AddTransition(new Transition(
            EStateType.Escaping,
            (condition) => Time.time >= timer || recoveryHealth != m_genericHealth.GetCurrentHealth() // Timer ends OR took damage
        ));

        parent.AddState(recovery);
    }

    private void Escaping_Init(StateMachine parent) {
        State escaping = new State(EStateType.Escaping, parent
        );

        escaping.AddTransition(new Transition(
            EStateType.Lurking,
            (condition) => false //TODO: implement transition
        ));

        parent.AddState(escaping);
    }

    private void Stun_Init(StateMachine parent) {
        State stun = new State(EStateType.Stun, parent
        );

        stun.AddTransition(new Transition(
            EStateType.Escaping,
            (condition) => false //TODO: implement transition
        ));

        parent.AddState(stun);
    }
    #endregion


    #endregion

    #region Utility
    public EStateType GetCurrentState() {
        return m_fsm.GetCurrentState();
    }

    public void ResetAgentPath() {
        if (agent.hasPath) {
            agent.ResetPath();
        }
    }
    #endregion
}
