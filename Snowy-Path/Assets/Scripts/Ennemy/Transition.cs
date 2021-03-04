using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition {

    public EStateType State { get; private set; }
    private readonly Predicate<Transition> condition;

    public Transition(EStateType to, Predicate<Transition> condition = null) {
        this.State = to;
        this.condition = condition;
    }

    public bool ShouldTransition() {
        return condition(this);
    }

}
