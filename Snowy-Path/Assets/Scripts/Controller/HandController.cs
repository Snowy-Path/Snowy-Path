using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;

public class HandController : MonoBehaviour {

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
            UseCurrentTool();
    }

    public void OnPreviousTool(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed)
            SwitchTool(-1);
    }

    public void OnNextTool(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed)
            SwitchTool(1);
    }

    public void OnEquipMap(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            EquipTool(typeof(JDCompassTest));
        }
    }

    public void OnEquipGun(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed)
            EquipTool(typeof(JDPistolTest));
    }

    public void OnEquipScope(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed)
            EquipTool(typeof(JDScopeTest));
    }
    #endregion

    private void UseCurrentTool() {
        if (tools.Length > 0)
            tools[currentToolIndex].PrimaryUse();
    }

    private void SwitchTool(int indexShift) {
        if (tools.Length == 0)
            return;

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

    private bool EquipTool(Type type) {
        if (TryGetToolIndex(type, out int index)) {
            currentToolIndex = index;
            HideTools();
            tools[currentToolIndex].ToggleDisplay(true);
            return true;
        }
        else
            return false;
    }

    private bool TryGetToolIndex(Type type, out int index) {

        for (int i = 0; i < tools.Length; i++) {
            if (tools[i].GetType() == type) {
                index = i;
                return true;
            }
        }

        index = -1;
        return false;
    }

}
