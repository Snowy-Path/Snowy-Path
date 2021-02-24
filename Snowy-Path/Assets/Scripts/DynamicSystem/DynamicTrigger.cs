using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicTrigger : MonoBehaviour {
    public DynamicSystem.EventStatusPair onEnterDynamicEvent;
    public DynamicSystem.EventStatusPair onExitDynamicEvent;


    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "Player") {
            DynamicSystem.RegisterEvent(onEnterDynamicEvent);
            Debug.Log($"Firing {onEnterDynamicEvent.dynamicEvent}={onEnterDynamicEvent.eventStatus}");
        }
    }

    private void OnTriggerExit(Collider other) {
        DynamicSystem.RegisterEvent(onExitDynamicEvent);
        Debug.Log($"Firing {onEnterDynamicEvent.dynamicEvent}={onExitDynamicEvent.eventStatus}");
    }
}
