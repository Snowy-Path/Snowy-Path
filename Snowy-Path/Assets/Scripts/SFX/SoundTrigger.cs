using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SoundTrigger : MonoBehaviour {

    enum TriggerType {
        Enter,
        Exit
    }

    [SerializeField]
    private TriggerType m_triggerType;

    private FMODUnity.StudioEventEmitter m_emitter;

    [SerializeField]
    [Min(0f)]
    private float m_cooldown = 60.0f;
    private float m_timer;

    private Transform m_emitterPosition;

    private void Start() {
        m_emitterPosition = transform.GetChild(0);
        m_timer = m_cooldown;
        m_emitter = m_emitterPosition.GetComponent<FMODUnity.StudioEventEmitter>();
    }

    private void Update() {
        m_timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (m_triggerType == TriggerType.Enter && m_timer >= m_cooldown) {
            PlayEvent();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (m_triggerType == TriggerType.Exit && m_timer >= m_cooldown) {
            PlayEvent();
        }
    }

    private void PlayEvent() {
        m_timer = 0.0f;
        m_emitter.Play();
    }

}
