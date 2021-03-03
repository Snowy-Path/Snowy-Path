using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : StateBase {
    internal override EStateType StateType => EStateType.Idle;

    public Idle(WolfController wolfController) : base(wolfController) { }

    internal override void OnEntry() {
        m_wolfController.waitingTimer = 0f;
    }

    internal override void OnExit() {
        m_wolfController.waitingTimer = 0f;
    }

    internal override void OnUpdate() {
        m_wolfController.waitingTimer += Time.deltaTime;
    }

    internal override void OnLateUpdate() {
        foreach (Transition transition in m_transitions) {
            if (transition.ShouldTransition()) {
                Parent.ChangeState(transition.State);
                break;
            }
        }
    }
}
