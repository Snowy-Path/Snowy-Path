﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

/// <summary>
/// Represent the Wolf Artificial Intelligence.
/// Controls and hold every behavior defined for the wolf.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(GenericHealth))]
[RequireComponent(typeof(Animator))]
public class WolfController : MonoBehaviour {

    #region Variables

    // Components retrieved in Awake method.
    private StateMachine<EWolfState> m_fsm;
    private GenericHealth m_genericHealth;
    private Animator m_animator;
    internal NavMeshAgent m_agent;

    // Generic timer used when needed (in multiple cases)
    private float m_timer;

    #region Seeing sense
    private bool m_isSeeingPlayer = false;
    public bool IsSeeingPlayer {
        get {
            return m_isSeeingPlayer;
        }
        internal set {
            m_isSeeingPlayer = value;
            if (m_isSeeingPlayer) {
                m_lastSaw = Time.time;
            }
        }
    }
    private float m_lastSaw = float.NegativeInfinity;
    #endregion

    #region Hearing sense
    private Vector3 m_lastPosition;
    public Vector3 LastPosition {
        get {
            return m_lastPosition;
        }
        internal set {
            m_lastPosition = value;
            heardPlayer = true;
        }
    }
    private bool heardPlayer = false;
    #endregion


    #region Idle
    [Header("Idle")]

    [Tooltip("Waiting time between 2 waypoints.")]
    [Min(0)]
    public float idleWaitingTime = 5f;
    #endregion

    #region MoveToWaypoint
    [Header("Waypoints")]

    [Tooltip("List of waypoints in the order of patrol.")]
    public List<Transform> waypoints;

    private int m_waypointIndex = 0;
    #endregion

    #region Aggro
    [Header("Aggro")]

    [Tooltip("Duration to loose the player if he is not seen by the SeeingSense.")]
    [Min(0)]
    public float loosingAggroDuration = 10f;

    private bool m_aggroFinished = false;
    #endregion

    #region Lurk
    [Header("Lurk")]

    [Tooltip("Safe distance to maintain while lurking the player.")]
    [Min(0)]
    public float safeDistance = 10f;

    [Tooltip("Minimum lurking time.")]
    [Min(0)]
    public float lurkingDurationMin = 15f;

    [Tooltip("Maximum lurking time.")]
    [Min(0)]
    public float lurkingDurationMax = 45f;

    private float m_lurkingTime; // Random lurking time
    #endregion

    #region Charge
    [Header("Charge")]

    [Tooltip("Charge speed when rushing towards the player.")]
    [Min(0)]
    public float chargeSpeed = 10f;

    private float m_normalSpeed;
    #endregion

    #region Attack
    [Header("Attack")]

    [Tooltip("Distance from which the Attack state and animation are triggered.")]
    [Min(0)]
    public float attackDistanceTrigger = 1.5f;

    private bool m_attackFinished = false;
    #endregion

    #region Recover
    [Header("Recover")]

    [Tooltip("Duration of the recovery state.")]
    [Min(0)]
    public float recoveryDuration = 4f;

    private int m_recoveryHealth = int.MinValue;
    #endregion

    #region Stun
    [Header("Duration of stun.")]
    public float stunDuration = 5f;
    #endregion

    #endregion

    #region Built-In Methods

    /// <summary>
    /// Retrieves components and create Hierarchichal Finite State Machine.
    /// </summary>
    private void Awake() {
        m_agent = GetComponent<NavMeshAgent>();
        m_genericHealth = GetComponent<GenericHealth>();
        m_animator = GetComponent<Animator>();
        m_normalSpeed = m_agent.speed;

        //After retrieving the NavMesh. Otherwise we'll get a NullPointerExcpetion when trying to access variable "agent"
        HFSMInitialization();
    }

    /// <summary>
    /// Called at each frame.
    /// Do needed frame update of the Finite State Machine.
    /// </summary>
    private void Update() {

#if DEBUG
        Debug.DrawLine(transform.position, m_agent.destination, Color.magenta);
        Vector3[] corners = m_agent.path.corners;
        for (int i = 1; i < corners.Length; i++) {
            Debug.DrawLine(corners[i - 1], corners[i], Color.yellow);
        }
        //Debug.Log(GetCurrentState());
#endif
        m_fsm.OnUpdate();
    }

    /// <summary>
    /// Called at each physics update.
    /// Do needed physics update of the Finite State Machine.
    /// </summary>
    private void FixedUpdate() {
        m_fsm.OnFixedUpdate();
    }

    /// <summary>
    /// Called at the end of each frame.
    /// Test transitions of the Finite State Machine.
    /// </summary>
    private void LateUpdate() {
        m_fsm.OnLateUpdate();
    }

    /// <summary>
    /// Called when gameobject is destroyed.
    /// Exit the Finite State Machine to stop the current state.
    /// </summary>
    private void OnDestroy() {
        m_fsm.OnExit();
    }
    #endregion

    #region HFSM
    /// <summary>
    /// Create the global Finite State machine that holds every sub-states and sub-state machines.
    /// Call creation of Patrol, Inspect, Aggro, Combat and Stun states as childs.
    /// </summary>
    private void HFSMInitialization() {
        m_fsm = new StateMachine<EWolfState>(EWolfState.None, EWolfState.Patrol);

        Patrol_Init(m_fsm);
        Inspect_Init(m_fsm);
        Aggro_Init(m_fsm);
        Combat_Init(m_fsm);
        Stun_Init(m_fsm);

        m_fsm.OnEntry();
    }


    #region Patrol
    /// <summary>
    /// Creates and add to <c>parent</c> the Patrol state machine.
    /// All transitions are created.
    /// Call creation of Idle and MoveToWaypoint states as childs.
    /// </summary>
    /// <param name="parent">The Patrol state machine's parent.</param>
    private void Patrol_Init(StateMachine<EWolfState> parent) {
        StateMachine<EWolfState> patrol = new StateMachine<EWolfState>(EWolfState.Patrol, EWolfState.MoveToWaypoint, parent);

        patrol.AddTransition(new Transition<EWolfState>(
            EWolfState.Aggro,
            (condition) => IsSeeingPlayer
        ));

        patrol.AddTransition(new Transition<EWolfState>(
            EWolfState.Inspect,
            (condition) => heardPlayer
        ));

        Idle_Init(patrol);
        MoveToWaypoint_Init(patrol);

        parent.AddState(patrol);
    }

    /// <summary>
    /// Creates and add to <c>parent</c> the Idle state.
    /// All logic and transitions of the Idle state are created.
    /// </summary>
    /// <param name="parent">The Idle state's parent</param>
    private void Idle_Init(StateMachine<EWolfState> parent) {
        State<EWolfState> idle = new State<EWolfState>(EWolfState.Idle, parent,
            onEntry: (state) => m_timer = Time.time + idleWaitingTime,
            onExit: (state) => {
                m_waypointIndex++;
                if (m_waypointIndex >= waypoints.Count) {
                    m_waypointIndex = 0;
                }
                m_timer = float.NegativeInfinity;
            }
        );

        idle.AddTransition(new Transition<EWolfState>(
            EWolfState.MoveToWaypoint,
            (condition) => Time.time >= m_timer
        ));

        parent.AddState(idle);
    }

    /// <summary>
    /// Creates and add to <c>parent</c> the MoveToWaypoint state.
    /// All logic and transitions of the MoveToWaypoint state are created.
    /// </summary>
    /// <param name="parent">The MoveToWaypoint state's parent</param>
    private void MoveToWaypoint_Init(StateMachine<EWolfState> parent) {
        State<EWolfState> moveToWaypoint = new State<EWolfState>(EWolfState.MoveToWaypoint, parent,
            onEntry: (state) => {
                heardPlayer = false; // Reset
                m_agent.SetDestination(waypoints[m_waypointIndex].position); // To set a destination before transitions are checked
            },
            //onUpdate: (state) => { // Can be removed for performance purposes
            //    agent.SetDestination(waypoints[waypointIndex].position); // To compute a shorter path each frame
            //},
            onExit: (state) => {
                ResetAgentPath();
            }
        );

        moveToWaypoint.AddTransition(new Transition<EWolfState>(
            EWolfState.Idle,
            (condition) => m_agent.remainingDistance < 0.5f
        ));

        parent.AddState(moveToWaypoint);
    }
    #endregion

    #region Inspect
    /// <summary>
    /// Creates and add to <c>parent</c> the Inspect state.
    /// All logic and transitions of the Inspect state are created.
    /// </summary>
    /// <param name="parent">The Inspect state's parent</param>
    private void Inspect_Init(StateMachine<EWolfState> parent) {
        State<EWolfState> inspect = new State<EWolfState>(EWolfState.Inspect, parent,
            onEntry: (state) => { // To set a destination before transitions are checked
                m_agent.SetDestination(LastPosition);
            },
            //onUpdate: (state) => { // Can be removed for performance purposes
            //    agent.SetDestination(SoundPosition); // To compute a shorter path each frame
            //},
            onExit: (state) => {
                ResetAgentPath();
            }
        );

        inspect.AddTransition(new Transition<EWolfState>(
            EWolfState.Aggro,
            (condition) => IsSeeingPlayer
        ));

        inspect.AddTransition(new Transition<EWolfState>(
            EWolfState.Patrol,
            (condition) => m_agent.remainingDistance < 0.5f
        ));

        parent.AddState(inspect);
    }
    #endregion

    #region Aggro
    /// <summary>
    /// Creates and add to <c>parent</c> the Aggro state.
    /// All logic and transitions of the Aggro state are created.
    /// </summary>
    /// <param name="parent">The Aggro state's parent</param>
    private void Aggro_Init(StateMachine<EWolfState> parent) {
        State<EWolfState> aggro = new State<EWolfState>(EWolfState.Aggro, parent,
            onEntry: (state) => {
                m_animator.SetTrigger("Aggro");
            },
            onExit: (state) => {
                m_aggroFinished = false;
            }
        );

        aggro.AddTransition(new Transition<EWolfState>(
            EWolfState.Combat,
            (condition) => m_aggroFinished
        ));

        parent.AddState(aggro);
    }
    #endregion

    #region Combat
    /// <summary>
    /// Creates and add to <c>parent</c> the Combat state machine.
    /// All transitions are created.
    /// Call creation of Lurk, Charge, Attack and Recover states as childs.
    /// </summary>
    /// <param name="parent">The Combat state machine's parent.</param>
    private void Combat_Init(StateMachine<EWolfState> parent) {
        StateMachine<EWolfState> combat = new StateMachine<EWolfState>(EWolfState.Combat, EWolfState.Lurk, parent);

        combat.AddTransition(new Transition<EWolfState>(
            EWolfState.Patrol,
            (condition) => (Time.time - m_lastSaw) >= loosingAggroDuration
        ));

        Lurk_Init(combat);
        Charge_Init(combat);
        Attack_Init(combat);
        Recover_Init(combat);

        parent.AddState(combat);
    }

    /// <summary>
    /// Creates and add to <c>parent</c> the Lurk state.
    /// All logic and transitions of the Lurk state are created.
    /// </summary>
    /// <param name="parent">The Lurk state's parent.</param>
    private void Lurk_Init(StateMachine<EWolfState> parent) {
        State<EWolfState> lurk = new State<EWolfState>(EWolfState.Lurk, parent,
            onEntry: (state) => {
                m_lurkingTime = Random.Range(lurkingDurationMin, lurkingDurationMax);
                m_timer = Time.time + m_lurkingTime;
            },
            onUpdate: (state) => {
                float distanceToTarget = Vector3.Distance(transform.position, m_lastPosition);
                m_agent.SetDestination(PositionToLurk(m_lastPosition));
            },
            onExit: (state) => {
                m_timer = float.NegativeInfinity;
            }
        );

        lurk.AddTransition(new Transition<EWolfState>(
            EWolfState.Charge,
            (condition) => Time.time >= m_timer && IsSeeingPlayer
        ));

        parent.AddState(lurk);
    }

    /// <summary>
    /// Computes the positions to lurk at <c>safeDistance</c> distance. 
    /// </summary>
    /// <param name="targetPosition">The position of the target. For example, target could be the Player.</param>
    /// <returns>The position to get and maintain safe distance.</returns>
    private Vector3 PositionToLurk(Vector3 targetPosition) {
        Vector3 forward = transform.position - targetPosition;
        forward.y = 0;
        transform.rotation = Quaternion.LookRotation(forward);
        Vector3 runToPosition = targetPosition + (transform.forward * safeDistance);
        return runToPosition;
    }

    /// <summary>
    /// Creates and add to <c>parent</c> the Charge state.
    /// All logic and transitions of the Charge state are created.
    /// </summary>
    /// <param name="parent">The Charge state's parent.</param>
    private void Charge_Init(StateMachine<EWolfState> parent) {
        State<EWolfState> charge = new State<EWolfState>(EWolfState.Charge, parent,
            onEntry: (state) => {
                m_agent.speed = chargeSpeed;
                m_agent.autoBraking = false;
                m_agent.SetDestination(m_lastPosition);
            },
            onUpdate: (state) => {
                m_agent.SetDestination(m_lastPosition); // Ensure we are always going to the Player's position 
            },
            onExit: (state) => {
                m_agent.speed = m_normalSpeed;
                m_agent.autoBraking = true;
                ResetAgentPath();
            }
        );

        charge.AddTransition(new Transition<EWolfState>(
            EWolfState.Attack,
            (condition) => m_agent.remainingDistance < attackDistanceTrigger
        ));

        parent.AddState(charge);
    }

    /// <summary>
    /// Creates and add to <c>parent</c> the Attack state.
    /// All logic and transitions of the Attack state are created.
    /// </summary>
    /// <param name="parent">The Attack state's parent.</param>
    private void Attack_Init(StateMachine<EWolfState> parent) {
        State<EWolfState> attack = new State<EWolfState>(EWolfState.Attack, parent,
            onEntry: (state) => {
                m_agent.speed = chargeSpeed;
                m_agent.autoBraking = false;
                m_animator.SetTrigger("Attack");
            },
            onUpdate: (state) => {
                m_agent.SetDestination(m_lastPosition); // Ensure we are always going to the Player's position 
            },
            onExit: (state) => {
                m_agent.speed = m_normalSpeed;
                m_agent.autoBraking = true;
                m_attackFinished = false;
            }
        );

        attack.AddTransition(new Transition<EWolfState>(
            EWolfState.Recover,
            (condition) => m_attackFinished
        ));

        parent.AddState(attack);
    }

    /// <summary>
    /// Creates and add to <c>parent</c> the Recover state.
    /// All logic and transitions of the Recover state are created.
    /// </summary>
    /// <param name="parent">The Recover state's parent.</param>
    private void Recover_Init(StateMachine<EWolfState> parent) {
        State<EWolfState> recover = new State<EWolfState>(EWolfState.Recover, parent,
            onEntry: (state) => {
                m_timer = Time.time + recoveryDuration;
                m_recoveryHealth = m_genericHealth.GetCurrentHealth();
            },
            onExit: (state) => {
                m_timer = float.NegativeInfinity;
                m_recoveryHealth = int.MinValue;
            }
        );

        recover.AddTransition(new Transition<EWolfState>(
            EWolfState.Lurk,
            (condition) => Time.time >= m_timer || m_recoveryHealth != m_genericHealth.GetCurrentHealth() // Timer ends OR took damage
        ));

        parent.AddState(recover);
    }

    #endregion

    #region Stun
    /// <summary>
    /// Creates and add to <c>parent</c> the Stun state.
    /// All logic and transitions of the Stun state are created.
    /// </summary>
    /// <param name="parent">The Stun state's parent</param>
    private void Stun_Init(StateMachine<EWolfState> parent) {
        State<EWolfState> stun = new State<EWolfState>(EWolfState.Stun, parent,
            onEntry: (state) => {
                m_timer = Time.time + stunDuration;
            },
            onExit: (state) => {
                m_timer = float.NegativeInfinity;
            }
        );

        stun.AddTransition(new Transition<EWolfState>(
            EWolfState.Combat,
            (condition) => Time.time >= m_timer
        ));

        parent.AddState(stun);
    }
    #endregion

    #endregion


    #region Utility

    #region Animation
    /// <summary>
    /// Called from the aggro animation as an animation event.
    /// Switch <c>aggroFinished</c> to true. Allows the Aggro state to transition to the next state.
    /// </summary>
    public void AggroAnimationFinished() {
        m_aggroFinished = true;
    }

    /// <summary>
    /// Called from the attack animation as an animation event.
    /// Switch <c>attackFinished</c> to true. Allows the Attack state to transition to the next state.
    /// </summary>
    public void AttackAnimationFinished() {
        m_attackFinished = true;
    }
    #endregion

    /// <summary>
    /// Return the current EWolfState value in the Finite State Machine.
    /// Used for Debug purpose.
    /// </summary>
    /// <returns></returns>
    internal EWolfState GetCurrentState() {
        return m_fsm.GetCurrentState();
    }

    /// <summary>
    /// Resets the agent path.
    /// </summary>
    private void ResetAgentPath() {
        if (m_agent.hasPath) {
            m_agent.ResetPath();
        }
    }

    /// <summary>
    /// Directly set the Stun state.
    /// Used when a Gun projectile hits. Called directly from the Gun script.
    /// </summary>
    internal void SetStunState() {
        m_fsm.SwitchState(EWolfState.Stun);
    }
    #endregion
}
