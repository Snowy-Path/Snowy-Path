using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : State {

    public EStateType defaultState = EStateType.None;
    private State m_currentState;
    private readonly Dictionary<EStateType, State> m_states;

    public StateMachine(EStateType stateType, StateMachine parent = null, Action<State> onEntry = null, Action<State> onExit = null, Action<State> onUpdate = null, Action<State> onFixedUpdate = null)
        : base(stateType, parent, onEntry, onExit, onUpdate, onFixedUpdate) {
        m_states = new Dictionary<EStateType, State>();
    }

    internal void AddState(State state) {
        m_states.Add(state.StateType, state);
    }

    internal void ChangeState(EStateType stateType) {

        if (m_currentState != null) {
            if (m_currentState.StateType == stateType) {
                return;
            }
            m_currentState.OnExit();
        }

        m_currentState = m_states[stateType];
        m_currentState.OnEntry();

        Debug.Log(m_currentState.StateType);
    }



    internal override void OnEntry() {
        base.OnEntry();
        ChangeState(defaultState);
    }

    internal override void OnExit() {
        base.OnExit();
        m_currentState.OnExit();
    }

    internal override void OnUpdate() {
        base.OnUpdate();
        m_currentState.OnUpdate();
    }

    internal override void OnFixedUpdate() {
        base.OnFixedUpdate();
        m_currentState.OnFixedUpdate();
    }

    internal override void OnLateUpdate() {
        base.OnLateUpdate();
        m_currentState.OnLateUpdate();
    }

}
