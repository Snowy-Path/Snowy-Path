using System.Collections.Generic;

public static class DynamicSystem {

    #region Specific structures
    [System.Serializable]
    public struct EventStatusPair {
        public DynamicEvent dynamicEvent;
        public bool eventStatus;
    }

    [System.Serializable]
    public struct DynamicCondition {
        public DynamicEvent dynamicEvent;
        public bool desiredStatus;
    }
    #endregion

    public delegate void RegisteredEventHandler();
    public static RegisteredEventHandler onEventRegistered;

    public static Dictionary<DynamicEvent, bool> RegisteredEvents {
        get {
            if (registeredEvents == null) { registeredEvents = new Dictionary<DynamicEvent, bool>(); }  //Init dictionary if null
            return registeredEvents;
        }
        private set { registeredEvents = value; }
    }
    private static Dictionary<DynamicEvent, bool> registeredEvents;

    /// <summary>
    /// Register an event
    /// </summary>
    /// <param name="eventStatusPair">The event-status to be registerd</param>
    public static void RegisterEvent(EventStatusPair eventStatusPair) {
        if (eventStatusPair.dynamicEvent == null)
            return;

        //Update event if its already in dictionary
        if (RegisteredEvents.ContainsKey(eventStatusPair.dynamicEvent)) {
            RegisteredEvents[eventStatusPair.dynamicEvent] = eventStatusPair.eventStatus;
        }
        else { //Add event to dictionary if not already in 
            RegisteredEvents.Add(eventStatusPair.dynamicEvent, eventStatusPair.eventStatus);
        }

        //Raise dynamic event registered
        onEventRegistered.Invoke();
    }

    /// <summary>
    /// Evaluate a condition
    /// </summary>
    /// <param name="condition"></param>
    /// <returns>Reult of evaluation</returns>
    public static bool CheckEvent(DynamicCondition condition) {

        //Return true if event is registered and has same value as the condition
        if (RegisteredEvents.TryGetValue(condition.dynamicEvent, out bool status)) {
            if (status == condition.desiredStatus)
                return true;
        }

        //Else return false
        return false;
    }

    public static void ResetDynamicSystem()
    {
        RegisteredEvents.Clear();
    }
}
