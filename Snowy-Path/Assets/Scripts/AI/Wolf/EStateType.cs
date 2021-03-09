using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStateType{
    None,
    Patrol,
    Idle,
    MoveToWaypoint,
    Inspecting,
    Combat,
    Aggro,
    Lurking,
    Charge,
    Attack,
    Recovery,
    Escaping,
    Stun
}