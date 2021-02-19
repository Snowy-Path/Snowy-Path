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

    /// <summary>
    /// Use active tool
    /// </summary>
    private void UseCurrentTool() {
        if (tools.Length > 0)
            tools[currentToolIndex].PrimaryUse();
    }

    /// <summary>
    /// Switch up or down current tool index then equip corresponding tool
    /// </summary>
    /// <param name="indexShift">Number of tools to shift</param>
    private void SwitchTool(int indexShift) {
        //Guard : if there is no tools, return
        if (tools.Length == 0)
            return;

        //shift the current index
        currentToolIndex += indexShift;

        //Handle index outside the array
        if (currentToolIndex >= tools.Length) {
            currentToolIndex = 0;
        }
        else if (currentToolIndex < 0) {
            currentToolIndex = tools.Length - 1;
        }

        HideTools();
        tools[currentToolIndex].ToggleDisplay(true);
    }

    /// <summary>
    /// Hide all carried tools
    /// </summary>
    private void HideTools() {
        foreach (IHandTool tool in tools) {
            tool.ToggleDisplay(false);
        }
    }

    /// <summary>
    /// Equip a tool according to type argument
    /// </summary>
    /// <param name="type">The type of tool to equip</param>
    /// <returns>Returns true if a tool was equiped, fase if not</returns>
    private bool EquipTool(Type type) {
        if (TryGetToolIndex(type, out int index)) {
            currentToolIndex = index;

            //Update display
            HideTools();
            tools[currentToolIndex].ToggleDisplay(true);
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// Find a tool matching type in among tools 
    /// </summary>
    /// <param name="type">The type of tool to found</param>
    /// <param name="index">Found tool index or -1 if no mathching tool</param>
    /// <returns>Returns true if a tool of matching type has been found</returns>
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
