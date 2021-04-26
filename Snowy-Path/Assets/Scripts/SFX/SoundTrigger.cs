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

    [SerializeField]
    [FMODUnity.EventRef]
    private string m_eventPath = "";

    [SerializeField]
    [Min(0f)]
    private float m_cooldown = 60.0f;
    private float m_timer;

    private Transform m_emitterPosition;

    private void Start() {
        m_emitterPosition = transform.GetChild(0);
        m_timer = m_cooldown;
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
        FMODUnity.RuntimeManager.PlayOneShot(m_eventPath, m_emitterPosition.position);
    }

}
