using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class WolfController : MonoBehaviour {

    #region Variables

    public Animator animator;
    private float timer;

    #region Seeing sense
    private bool isSeeingPlayer = false;
    public bool IsSeeingPlayer {
        get {
            return isSeeingPlayer;
        }
        internal set {
            isSeeingPlayer = value;
            if (isSeeingPlayer) {
                lastSaw = Time.time;
            }
        }
    }
    private float lastSaw = float.NegativeInfinity;
    #endregion

    #region Hearing sense
    private Vector3 lastPosition;
    public Vector3 LastPosition {
        get {
            return lastPosition;
        }
        internal set {
            lastPosition = value;
            heardPlayer = true;
        }
    }

    private bool heardPlayer;
    #endregion


    #region Patrol
    #endregion

    #region Idle
    [Min(0)]
    public float idleWaitingTime = 5f;
    #endregion

    #region MoveToWaypoint
    public List<Transform> waypoints;
    private int waypointIndex = 0;
    #endregion

    #region Aggro
    public float loosingAggroDuration = 10f;
    private bool aggroFinished = false;
    public void AggroAnimationFinished() {
        aggroFinished = true;
    }
    #endregion

    #region Lurk
    public float lurkingDistance = 10f;
    public float lurkingTimeMin = 15f;
    public float lurkingTimeMax = 45f;
    private float lurkingTime;
    public float safeDistance = 10f;
    #endregion

    #region Charge
    public float chargeSpeed;
    private float normalSpeed;
    #endregion

    #region Attack
    public float attackTriggerRange = 1.5f;
    private bool attackFinished = false;
    public void AttackAnimationFinished() {
        attackFinished = true;
    }
    #endregion

    #region Recover
    public float recoveryDuration = 4f;
    private int recoveryHealth = int.MinValue;
    #endregion

    #region Stun
    public float stunDuration = 5f;
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
        Inspect_Init(m_fsm);
        Aggro_Init(m_fsm);
        Combat_Init(m_fsm);
        Stun_Init(m_fsm);

        m_fsm.OnEntry();
    }


    #region Patrol
    private void Patrol_Init(StateMachine parent) {
        StateMachine patrol = new StateMachine(EStateType.Patrol, EStateType.MoveToWaypoint, parent);

        patrol.AddTransition(new Transition(
            EStateType.Aggro,
            (condition) => IsSeeingPlayer
        ));

        patrol.AddTransition(new Transition(
            EStateType.Inspect,
            (condition) => heardPlayer
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
                heardPlayer = false; // Reset
                agent.SetDestination(waypoints[waypointIndex].position); // To set a destination before transitions are checked
            },
            //onUpdate: (state) => { // Can be removed for performance purposes
            //    agent.SetDestination(waypoints[waypointIndex].position); // To compute a shorter path each frame
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

    #region Inspect
    private void Inspect_Init(StateMachine parent) {
        State inspect = new State(EStateType.Inspect, parent,
            onEntry: (state) => { // To set a destination before transitions are checked
                agent.SetDestination(LastPosition);
            },
            //onUpdate: (state) => { // Can be removed for performance purposes
            //    agent.SetDestination(SoundPosition); // To compute a shorter path each frame
            //},
            onExit: (state) => {
                ResetAgentPath();
            }
        );

        inspect.AddTransition(new Transition(
            EStateType.Aggro,
            (condition) => IsSeeingPlayer
        ));

        inspect.AddTransition(new Transition(
            EStateType.Patrol,
            (condition) => agent.remainingDistance < 0.5f
        ));

        parent.AddState(inspect);
    }
    #endregion

    #region Aggro
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
            EStateType.Combat,
            (condition) => aggroFinished
        ));

        parent.AddState(aggro);
    }
    #endregion

    #region Combat
    private void Combat_Init(StateMachine parent) {
        StateMachine combat = new StateMachine(EStateType.Combat, EStateType.Lurk, parent);

        combat.AddTransition(new Transition(
            EStateType.Patrol,
            (condition) => (Time.time - lastSaw) >= loosingAggroDuration
        ));

        Lurk_Init(combat);
        Charge_Init(combat);
        Attack_Init(combat);
        Recover_Init(combat);
        Stun_Init(combat);

        parent.AddState(combat);
    }

    private void Lurk_Init(StateMachine parent) {
        State lurk = new State(EStateType.Lurk, parent,
            onEntry: (state) => {
                lurkingTime = Random.Range(lurkingTimeMin, lurkingTimeMax);
                timer = Time.time + lurkingTime;
            },
            onUpdate: (state) => {
                float distanceToTarget = Vector3.Distance(transform.position, lastPosition);
                agent.SetDestination(PositionToLurk(lastPosition));
            },
            onExit: (state) => {
                timer = float.NegativeInfinity;
            }
        );

        lurk.AddTransition(new Transition(
            EStateType.Charge,
            (condition) => Time.time >= timer && IsSeeingPlayer
        ));

        parent.AddState(lurk);
    }

    private Vector3 PositionToLurk(Vector3 targetPosition) {
        Vector3 forward = transform.position - targetPosition;
        forward.y = 0;
        transform.rotation = Quaternion.LookRotation(forward);
        Vector3 runToPosition = targetPosition + (transform.forward * safeDistance);
        return runToPosition;
    }

    private void Charge_Init(StateMachine parent) {
        State charge = new State(EStateType.Charge, parent,
            onEntry: (state) => {
                agent.speed = chargeSpeed;
                agent.autoBraking = false;
                agent.SetDestination(lastPosition); //NullPointerExecption
            },
            onUpdate: (state) => {
                agent.SetDestination(lastPosition);
            },
            onExit: (state) => {
                agent.speed = normalSpeed;
                ResetAgentPath();
            }
        );

        charge.AddTransition(new Transition(
            EStateType.Attack,
            (condition) => agent.remainingDistance < attackTriggerRange
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
            EStateType.Recover,
            (condition) => attackFinished
        ));

        parent.AddState(attack);
    }

    private void Recover_Init(StateMachine parent) {
        State recover = new State(EStateType.Recover, parent,
            onEntry: (state) => {
                timer = Time.time + recoveryDuration;
                recoveryHealth = m_genericHealth.GetCurrentHealth();
            },
            onExit: (state) => {
                timer = float.NegativeInfinity;
                recoveryHealth = int.MinValue;
            }
        );

        recover.AddTransition(new Transition(
            EStateType.Lurk,
            (condition) => Time.time >= timer || recoveryHealth != m_genericHealth.GetCurrentHealth() // Timer ends OR took damage
        ));

        parent.AddState(recover);
    }

    #endregion

    #region Stun
    private void Stun_Init(StateMachine parent) {
        State stun = new State(EStateType.Stun, parent,
            onEntry: (state) => {
                timer = Time.time + stunDuration;
            },
            onExit: (state) => {
                timer = float.NegativeInfinity;
            }
        );

        stun.AddTransition(new Transition(
            EStateType.Combat,
            (condition) => Time.time >= timer || recoveryHealth != m_genericHealth.GetCurrentHealth()
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
