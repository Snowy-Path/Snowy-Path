using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class HandController : MonoBehaviour {

    enum HandPosition {
        None,
        Compass,
        Pistol,
        Scope,
        Torch
    }

    private IHandTool[] tools;
    private int currentToolIndex = -1;

    // Start is called before the first frame update
    void Start() {
        tools = GetComponentsInChildren<MonoBehaviour>().OfType<IHandTool>().ToArray();
        SwitchTool(1);
    }

    #region INPUT SYSTEM EVENTS
    public void OnUseTool(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed)
            tools[currentToolIndex].PrimaryUse();
    }

    public void OnPreviousTool(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed)
            SwitchTool(-1);
    }

    public void OnNextTool(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed)
            SwitchTool(1);
    }
    #endregion

    private void SwitchTool(int indexShift) {
        currentToolIndex += indexShift;

        if (currentToolIndex >= tools.Length) {
            currentToolIndex = 0;
        }
        else if (currentToolIndex < 0) {
            currentToolIndex = tools.Length - 1;
        }

        HideTools();
        tools[currentToolIndex].ToggleDisplay(true);
    }

    private void HideTools() {
        foreach (IHandTool tool in tools) {
            tool.ToggleDisplay(false);
        }
    }

}
