using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlStelePuzzle : MonoBehaviour {

    public bool IsActive { private set; get; }
    private Transform m_child;
    private ParticleSystem[] m_particleSystems;

    private void Start() {
        IsActive = false;
        m_child = transform.GetChild(0);
        m_particleSystems = m_child.GetComponentsInChildren<ParticleSystem>(true);
    }

    public void SwitchActivation() {
        m_child.gameObject.SetActive(!IsActive);
        IsActive = !IsActive;
    }

    internal void LightBowl() {
        m_child.gameObject.SetActive(true);
    }

    internal void ChangeParticleColor(Gradient color) {
        foreach(var ps in m_particleSystems) {
            var col = ps.colorOverLifetime;
            col.color = color;
        }
    }
}
