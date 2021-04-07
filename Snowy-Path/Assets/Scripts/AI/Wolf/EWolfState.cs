/// <summary>
/// Enumeration referencing every state used in the Wolf HFSM (Hierarchichal Finite State Machine).
/// </summary>
public enum EWolfState{
    None,
    Patrol,
    Idle,
    MoveToWaypoint,
    Inspect,
    Aggro,
    Combat,
    Lurk,
    Charge,
    Attack,
    Recover,
    Stun,
    Death
}