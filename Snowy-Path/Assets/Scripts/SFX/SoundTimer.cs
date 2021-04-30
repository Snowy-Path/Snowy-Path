using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FMODUnity.StudioEventEmitter))]
public class SoundTimer : MonoBehaviour {

    private FMODUnity.StudioEventEmitter m_emitter;

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

    private void Start() {
        m_emitter = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    private void Update() {

        m_timer += Time.deltaTime;
        if (m_timer >= m_internalCooldown) {

            m_emitter.Play();
            m_timer = 0.0f;

            if (m_randomCooldown) {
                m_internalCooldown = Random.Range(m_cooldownMin, m_cooldownMax);
            }
        }
    }

}
