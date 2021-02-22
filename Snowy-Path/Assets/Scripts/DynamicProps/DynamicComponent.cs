using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DynamicComponent : MonoBehaviour
{
    public DynamicSystem.EventStatusPair dynamicEvent;

    public void SendDynamicEvent() {    //To be call by interactable onUse UnityEvent
        DynamicSystem.RegisterEvent(dynamicEvent);
    }
}
