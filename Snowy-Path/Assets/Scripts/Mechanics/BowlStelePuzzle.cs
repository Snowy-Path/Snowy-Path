using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlStelePuzzle : MonoBehaviour {

    public bool IsActive { private set; get; }
    private Transform m_child;

    private void Start() {
        m_child = transform.GetChild(0);
        IsActive = false;
    }

    public void SwitchActivation() {
        m_child.gameObject.SetActive(!IsActive);
        IsActive = !IsActive;
    }

    internal void LightBowl() {
        m_child.gameObject.SetActive(true);
    }
}
