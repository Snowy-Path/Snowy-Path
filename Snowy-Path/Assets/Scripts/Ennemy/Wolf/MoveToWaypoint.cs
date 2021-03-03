using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToWaypoint : StateBase {
    internal override EStateType StateType => EStateType.MoveToWaypoint;

    public MoveToWaypoint(WolfController wolfController) : base(wolfController) {}
}
