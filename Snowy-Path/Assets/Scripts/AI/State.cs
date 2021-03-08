using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class State {

    #region Variables
    internal EStateType StateType { get; }
    private StateMachine Parent { get; }
    private List<Transition> m_transitions;

    private Action<State> onEntry;
    private Action<State> onExit;
    private Action<State> onUpdate;
    private Action<State> onFixedUpdate;
    #endregion

    #region Constructor
    public State(EStateType stateType, StateMachine parent = null, Action<State> onEntry = null, Action<State> onExit = null, Action<State> onUpdate = null, Action<State> onFixedUpdate = null) {
        this.StateType = stateType;
        this.Parent = parent;
        this.m_transitions = new List<Transition>();
        this.onEntry = onEntry;
        this.onExit = onExit;
        this.onUpdate = onUpdate;
        this.onFixedUpdate = onFixedUpdate;
    }
    #endregion

    #region State logic methods
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
    #endregion

    #region Utility methods
    internal virtual void AddTransition(Transition transition) {
        m_transitions.Add(transition);
    }

    internal virtual EStateType GetCurrentState() {
        return StateType;
    }
    #endregion
}
