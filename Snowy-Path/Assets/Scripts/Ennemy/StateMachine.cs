using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StateMachine : StateBase {

    public EStateType defaultState;
    private StateBase m_currentState;
    private Dictionary<EStateType, StateBase> m_states;

    internal override EStateType StateType => EStateType.None;

    public StateMachine(WolfController wolfController) : base(wolfController) {}

    internal override void OnEntry() {
        m_currentState = m_states[defaultState];
        m_currentState.OnEntry();
    }

    internal override void OnExit() {
        m_currentState.OnExit();
    }

    internal override void OnUpdate() {
        m_currentState.OnUpdate();
    }

    internal void AddState(StateBase state) {
        m_states.Add(state.StateType, state);
    }

    internal void ChangeState(EStateType stateType) {
        if (m_currentState.StateType == stateType) {
            return;
        }

        m_currentState.OnExit();
        m_currentState = m_states[stateType];
        m_currentState.OnEntry();
    }

    internal override void OnFixedUpdate() {
        m_currentState.OnFixedUpdate();
    }

    internal override void OnLateUpdate() {
        m_currentState.OnLateUpdate();
    }

}
