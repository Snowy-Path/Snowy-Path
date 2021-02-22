using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSet : MonoBehaviour {
    public GameObject defaultSet;
    public GameObject alternativeSet;

    public List<DynamicSystem.DynamicCondition> conditions;

    // Start is called before the first frame update
    void Start() {
        UpdateDisplay();
        DynamicSystem.onEventRegistered += UpdateDisplay;
    }

    // Update is called once per frame
    void Update() {

    }

    public void UpdateDisplay() {
        bool result = true;
        foreach (DynamicSystem.DynamicCondition condition in conditions) {
            if (!DynamicSystem.CheckEvent(condition)) {
                result = false;
                break;
            }
        }

        if (defaultSet)
            defaultSet.gameObject.SetActive(!result);
        if (alternativeSet)
            alternativeSet.gameObject.SetActive(result);
    }
}
