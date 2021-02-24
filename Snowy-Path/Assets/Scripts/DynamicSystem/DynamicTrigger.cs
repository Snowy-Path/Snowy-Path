using UnityEngine;

public class DynamicTrigger : MonoBehaviour {

    public DynamicSystem.EventStatusPair onEnterDynamicEvent;
    public DynamicSystem.EventStatusPair onExitDynamicEvent;

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "Player") {
            DynamicSystem.RegisterEvent(onEnterDynamicEvent); //Send dynamic event
            Debug.Log($"Firing {onEnterDynamicEvent.dynamicEvent}={onEnterDynamicEvent.eventStatus}");
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            DynamicSystem.RegisterEvent(onExitDynamicEvent); //Send dynamic event
            Debug.Log($"Firing {onEnterDynamicEvent.dynamicEvent}={onExitDynamicEvent.eventStatus}");
        }
    }
}
