using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTimer : MonoBehaviour {

    [SerializeField]
    [FMODUnity.EventRef]
    private string m_eventPath = "";

    [SerializeField]
    [Min(0f)]
    private float m_cooldown = 60.0f;
    private float m_timer;

    [SerializeField] private bool m_randomCooldown;

    [SerializeField]
    [Min(0f)]
    private float m_cooldownMin = 60.0f;
    [SerializeField]
    [Min(0f)]
    private float m_cooldownMax = 120.0f;

    private float m_internalCooldown;

    private void Awake() {
        if (m_randomCooldown) {
            m_internalCooldown = Random.Range(m_cooldownMin, m_cooldownMax);
        } else {
            m_internalCooldown = m_cooldown;
        }
    }

    private void Update() {

        m_timer += Time.deltaTime;
        if (m_timer >= m_internalCooldown) {

            FMODUnity.RuntimeManager.PlayOneShot(m_eventPath, transform.position);
            m_timer = 0.0f;

            if (m_randomCooldown) {
                m_internalCooldown = Random.Range(m_cooldownMin, m_cooldownMax);
            }
        }
    }

}
