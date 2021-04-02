using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAnimatorUtility : MonoBehaviour {

    [SerializeField]
    private WolfController m_wolfController;

    public void AggroFinished() {
        m_wolfController.AggroFinished = true;
    }

    public void StartAttack() {
        m_wolfController.StartAttack();
    }
    public void StopAttack() {
        m_wolfController.StopAttack();
        m_wolfController.AttackFinished = true;
    }

    public void Dead() {
        m_wolfController.Dead();
    }
}
