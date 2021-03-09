using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearingSenseEmitter : MonoBehaviour {

    private Dictionary<int, HearingSenseReceiver> m_receiverDic;

    private void Awake() {
        m_receiverDic = new Dictionary<int, HearingSenseReceiver>();
    }

    private void OnDisable() {
        m_receiverDic.Clear(); //Clearing the dictionary when disabling the component. When reactivated, ensure we do not register AGAIN
    }

    private void OnTriggerEnter(Collider other) {
        HearingSenseReceiver receiver = other.gameObject.GetComponent<HearingSenseReceiver>();
        // Guard
        if (receiver == null) {
            return;
        }
        m_receiverDic.Add(receiver.GetInstanceID(), receiver);
    }

    private void OnTriggerExit(Collider other) {
        HearingSenseReceiver receiver = other.gameObject.GetComponent<HearingSenseReceiver>();
        // Guard
        if (receiver == null) {
            return;
        }
        m_receiverDic.Remove(receiver.GetInstanceID());
    }

    public void Emit() {
        foreach (HearingSenseReceiver receiver in m_receiverDic.Values) {
            receiver.Receive(transform.position);
        }
    }

}
