using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandController : MonoBehaviour {

    private HandTool[] tools;
    private int currentToolIndex;

    // Start is called before the first frame update
    void Start() {
        tools = GetComponentsInChildren<HandTool>();
    }

    public void OnUseTool(InputAction.CallbackContext context) {
        tools[currentToolIndex].UseTool();
    }

    public void OnPreviousTool(InputAction.CallbackContext context) {
        SwitchTool(-1);
    }

    public void OnNextTool(InputAction.CallbackContext context) {
        SwitchTool(1);
    }

    private void SwitchTool(int indexShift) {
        currentToolIndex += indexShift;

        if (currentToolIndex >= tools.Length) {
            currentToolIndex = 0;
        }
        else if (currentToolIndex < 0) {
            currentToolIndex = tools.Length - 1;
        }

        HideTools();
        tools[currentToolIndex].gameObject.SetActive(true);
    }

    private void HideTools() {
        foreach (HandTool tool in tools) {
            tool.gameObject.SetActive(false);
        }
    }

}
