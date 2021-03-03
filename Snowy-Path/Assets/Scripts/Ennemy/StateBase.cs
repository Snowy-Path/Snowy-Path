using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class StateBase {

    protected WolfController m_wolfController;
    protected List<Transition> m_transitions;
    public StateMachine Parent { get; private set; }

    internal abstract EStateType StateType { get; }

    protected StateBase(WolfController wolfController) {
        m_wolfController = wolfController;
        m_transitions = new List<Transition>();
    }

    internal virtual void OnEntry() { }
    internal virtual void OnExit() { }
    internal virtual void OnUpdate() { }
    internal virtual void OnFixedUpdate() { }
    internal virtual void OnLateUpdate() { }

    internal virtual void AddTransition(Transition transition) {
        m_transitions.Add(transition);
    }

}
