using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : State {

    internal EStateType defaultState { get; private set; }
    private State m_currentState;
    private Dictionary<EStateType, State> m_states;

    public StateMachine(EStateType stateType, EStateType defaultState, StateMachine parent = null, Action<State> onEntry = null, Action<State> onExit = null, Action<State> onUpdate = null, Action<State> onFixedUpdate = null)
        : base(stateType, parent, onEntry, onExit, onUpdate, onFixedUpdate) {
        this.defaultState = defaultState;
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
    }



    internal override void OnEntry() {
        base.OnEntry();
        ChangeState(defaultState);
    }

    internal override void OnExit() {
        base.OnExit();
        m_currentState.OnExit();
        m_currentState = null;
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
        m_currentState.OnLateUpdate();
        base.OnLateUpdate();
    }

    internal override EStateType GetCurrentState() {
        return m_currentState.GetCurrentState();
    }

}
