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
    private Transform m_emitterPosition;

    [SerializeField]
    [Min(0f)]
    private float m_cooldown = 60.0f;
    private float m_timer = 0.0f;

    private void Awake() {
        m_timer = m_cooldown;
    }

    private void Start() {
        m_emitterPosition = transform.GetChild(0);
        m_emitter = m_emitterPosition.GetComponent<FMODUnity.StudioEventEmitter>();
    }

    private void Update() {
        m_timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (m_triggerType == TriggerType.Enter && m_timer >= m_cooldown && other.CompareTag("Player")) {
            PlayEvent();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (m_triggerType == TriggerType.Exit && m_timer >= m_cooldown && other.CompareTag("Player")) {
            PlayEvent();
        }
    }

    private void PlayEvent() {
        m_timer = 0.0f;
        m_emitter.Play();
    }

}
