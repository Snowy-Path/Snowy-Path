using System.Collections;
using System.Collections.Generic;

public static class DynamicSystem {

    #region Specific structures
    [System.Serializable]
    public struct EventStatusPair {
        public EDynamicEvent dynamicEvent;
        public bool eventStatus;
    }

    [System.Serializable]
    public struct DynamicCondition {
        public EDynamicEvent dynamicEvent;
        public bool desiredStatus;
    }
    #endregion

    public delegate void RegisteredEventHandler();
    public static RegisteredEventHandler onEventRegistered;

    public static Dictionary<EDynamicEvent, bool> RegisteredEvents {
        get {
            if (registeredEvents == null) { registeredEvents = new Dictionary<EDynamicEvent, bool>(); }
            return registeredEvents;
        }
        private set { registeredEvents = value; }
    }
    private static Dictionary<EDynamicEvent, bool> registeredEvents;

    public static void RegisterEvent(EventStatusPair eventStatusPair) {
        if (RegisteredEvents.ContainsKey(eventStatusPair.dynamicEvent)) {
            RegisteredEvents[eventStatusPair.dynamicEvent] = eventStatusPair.eventStatus;
        }
        else {
            RegisteredEvents.Add(eventStatusPair.dynamicEvent, eventStatusPair.eventStatus);
        }

        onEventRegistered.Invoke();
    }

    public static bool CheckEvent(DynamicCondition condition) {

        if (RegisteredEvents.TryGetValue(condition.dynamicEvent, out bool status)) {
            if (status == condition.desiredStatus)
                return true;
        }

        return false;
    }
}
