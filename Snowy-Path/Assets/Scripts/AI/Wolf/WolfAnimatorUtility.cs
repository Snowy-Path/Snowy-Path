using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAnimatorUtility : MonoBehaviour {

    [SerializeField]
    private WolfController m_wolfController;

    public void Dead() {
        m_wolfController.DestroyItself();
    }
}
