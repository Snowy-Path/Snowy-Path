using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class State {
    internal EStateType StateType { get; }
    private StateMachine Parent { get; }
    private readonly List<Transition> m_transitions;

    public State(EStateType stateType, StateMachine parent = null, Action<State> onEntry = null, Action<State> onExit = null, Action<State> onUpdate = null, Action<State> onFixedUpdate = null) {
        this.StateType = stateType;
        this.Parent = parent;
        this.m_transitions = new List<Transition>();
        this.onEntry = onEntry;
        this.onExit = onExit;
        this.onUpdate = onUpdate;
        this.onFixedUpdate = onFixedUpdate;
    }

    internal virtual void AddTransition(Transition transition) {
        m_transitions.Add(transition);
    }

    private readonly Action<State> onEntry;
    private readonly Action<State> onExit;
    private readonly Action<State> onUpdate;
    private readonly Action<State> onFixedUpdate;

    internal virtual void OnEntry() {
        onEntry?.Invoke(this);
    }

    internal virtual void OnExit() {
        onExit?.Invoke(this);
    }

    internal virtual void OnUpdate() {
        onUpdate?.Invoke(this);
    }

    internal virtual void OnFixedUpdate() {
        onFixedUpdate?.Invoke(this);
    }

    internal virtual void OnLateUpdate() {
        foreach (Transition transition in m_transitions) {
            if (transition.ShouldTransition()) {
                Parent.ChangeState(transition.State);
                break;
            }
        }
    }
}
