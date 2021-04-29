using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FMODUnity.StudioEventEmitter))]
public class SoundCycle : MonoBehaviour {

    private FMODUnity.StudioEventEmitter m_emitter;

    [SerializeField]
    private bool m_hasDelay = false;

    [SerializeField]
    private float m_startDelay = 1.0f;

    [SerializeField]
    private float m_cycleDelay = 5.0f;

    IEnumerator Start() {
        m_emitter = GetComponent<FMODUnity.StudioEventEmitter>();
        
        if (m_hasDelay) {
            yield return new WaitForSeconds(m_startDelay);
        }

        while (true) {
            m_emitter.Play();
            yield return new WaitForSeconds(m_cycleDelay);
        }
    }
}
