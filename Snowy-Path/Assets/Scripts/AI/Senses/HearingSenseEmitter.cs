using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Emitter script of the Hearing Sense. The emitter can be anything related to the player. (A Gun firing for example).
/// Must be in a specific gameobject child of the AI with a Collider set on isTrigger and with the Layer HearingSense.
/// </summary>
public class HearingSenseEmitter : MonoBehaviour {

    // Holds every HearingSenseReceiver in range. Updated with the Trigger methods.
    private HashSet<HearingSenseReceiver> m_receiverDic;

    /// <summary>
    /// Creates the HashSet of HearingSenseReceiver.
    /// </summary>
    private void Awake() {
        m_receiverDic = new HashSet<HearingSenseReceiver>();
    }

    /// <summary>
    /// Clears the HashSet when this component is disabled.
    /// </summary>
    private void OnDisable() {
        m_receiverDic.Clear(); //Clearing the dictionary when disabling the component. When reactivated, ensure we do not register AGAIN
    }

    /// <summary>
    /// Register every HearingSenseReceiver entering the range.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other) {
        HearingSenseReceiver receiver = other.gameObject.GetComponent<HearingSenseReceiver>();
        // Guard
        if (receiver == null) {
            return;
        }
        m_receiverDic.Add(receiver);
    }

    /// <summary>
    /// Register every HearingSenseReceiver leaving the range.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other) {
        HearingSenseReceiver receiver = other.gameObject.GetComponent<HearingSenseReceiver>();
        // Guard
        if (receiver == null) {
            return;
        }
        m_receiverDic.Remove(receiver);
    }

    /// <summary>
    /// For each HearingSenseReceive in range, emits the sound position.
    /// </summary>
    public void Emit() {
        foreach (HearingSenseReceiver receiver in m_receiverDic) {
            receiver.Receive(transform.position);
        }
    }

}
