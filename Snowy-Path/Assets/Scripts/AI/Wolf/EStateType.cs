﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStateType{
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
    Stun
}