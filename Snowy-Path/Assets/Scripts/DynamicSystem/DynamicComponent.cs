using UnityEngine;

public class DynamicComponent : MonoBehaviour
{
    public DynamicSystem.EventStatusPair dynamicEvent;

    /// <summary>
    /// Send chosen dynamicEvent to DynamicSystem
    /// </summary>
    public void SendDynamicEvent() {    //To be call by interactable onUse or DynamicTrigger onEnter
        DynamicSystem.RegisterEvent(dynamicEvent);
        Debug.Log($"Firing {dynamicEvent.dynamicEvent}={dynamicEvent.eventStatus}");
    }
}
