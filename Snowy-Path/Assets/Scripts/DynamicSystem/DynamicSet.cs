using System.Collections.Generic;
using UnityEngine;

public class DynamicSet : MonoBehaviour {

    public GameObject defaultSet;
    public GameObject alternativeSet;

    public List<DynamicSystem.DynamicCondition> conditions;

    void Start() {
        UpdateDisplay();
        DynamicSystem.onEventRegistered += UpdateDisplay; //Bind display change with DynamicSystem
    }

    /// <summary>
    /// Evaluate conditions and display alternative set if they are all validated. Display default set if not.
    /// </summary>
    public void UpdateDisplay() {
        bool result = true;

        //Evaluate all conditions, if at least one is not validated, then result is false
        foreach (DynamicSystem.DynamicCondition condition in conditions) {
            if (!DynamicSystem.CheckEvent(condition)) {
                result = false;
                break;
            }
        }

        //Display set accordingly
        if (defaultSet)
            defaultSet.gameObject.SetActive(!result);
        if (alternativeSet)
            alternativeSet.gameObject.SetActive(result);
    }
}
