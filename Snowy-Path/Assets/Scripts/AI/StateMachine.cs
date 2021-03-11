using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<TEnum> : State<TEnum> where TEnum : System.Enum {

    internal TEnum defaultState { get; private set; }
    private State<TEnum> m_currentState;
    private Dictionary<TEnum, State<TEnum>> m_states;

    public StateMachine(TEnum stateType, TEnum defaultState, StateMachine<TEnum> parent = null, Action<State<TEnum>> onEntry = null, Action<State<TEnum>> onExit = null, Action<State<TEnum>> onUpdate = null, Action<State<TEnum>> onFixedUpdate = null)
        : base(stateType, parent, onEntry, onExit, onUpdate, onFixedUpdate) {
        this.defaultState = defaultState;
        m_states = new Dictionary<TEnum, State<TEnum>>();
    }

    internal void AddState(State<TEnum> state) {
        m_states.Add(state.StateType, state);
    }

    internal void SwitchState(TEnum stateType) {

        if (m_currentState != null) {
            if (EqualityComparer<TEnum>.Default.Equals(m_currentState.StateType, stateType)) {
                return;
            }
            m_currentState.OnExit();
        }

        m_currentState = m_states[stateType];
        m_currentState.OnEntry();
    }



    internal override void OnEntry() {
        base.OnEntry();
        SwitchState(defaultState);
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

    internal override TEnum GetCurrentState() {
        return m_currentState.GetCurrentState();
    }

}
