using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;

public class HandController : MonoBehaviour {

    public IHandTool CurrentTool {
        get {
            if (currentToolIndex > 0) {
                return tools[currentToolIndex];
            }
            else
                return null;
        }
    }

    [SerializeField] Animator handsAnimator;
    private IHandTool[] tools;
    private int currentToolIndex = -1;

    // Start is called before the first frame update
    void Start() {
        tools = GetComponentsInChildren<IHandTool>();
        SwitchTool(1);
    }

    #region INPUT SYSTEM EVENTS
    public void OnPrimaryUseTool(InputAction.CallbackContext context) {
        switch (context.phase) {
            case InputActionPhase.Started:
                PrimaryUseCurrentTool();
                break;
            case InputActionPhase.Canceled:
                CancelUseCurrentTool();
                break;
        }
    }

    public void OnSecondaryUseTool(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed)
            SecondaryUseCurrentTool();
    }

    public void OnScrollTool(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            int shift = (int)context.ReadValue<float>();
            SwitchTool(shift);
        }
    }

    public void OnEquipMap(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            EquipTool(EToolType.MapCompass);
        }
    }

    public void OnEquipGun(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed)
            EquipTool(EToolType.Pistol);
    }

    public void OnEquipScope(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed)
            EquipTool(EToolType.Scope);
    }
    #endregion

    /// <summary>
    /// Use active tool primary function
    /// </summary>
    private void PrimaryUseCurrentTool() {
        if (tools.Length > 0)
            tools[currentToolIndex].StartPrimaryUse();
    }

    /// <summary>
    /// Use active tool secondary function
    /// </summary>
    private void SecondaryUseCurrentTool() {
        if (tools.Length > 0)
            tools[currentToolIndex].SecondaryUse();
    }


    /// <summary>
    /// Cancel active tool use
    /// </summary>
    private void CancelUseCurrentTool() {
        if (tools.Length > 0)
            tools[currentToolIndex].CancelPrimaryUse();
    }

    /// <summary>
    /// Switch up or down current tool index then equip corresponding tool
    /// </summary>
    /// <param name="indexShift">Number of tools to shift</param>
    private void SwitchTool(int indexShift) {
        //Guard : if there is no tools, return
        if (tools.Length == 0 || (CurrentTool != null && CurrentTool.IsBusy))
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
        handsAnimator.SetTrigger("SwitchTool");
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
    private bool EquipTool(EToolType type) {
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
    private bool TryGetToolIndex(EToolType type, out int index) {

        for (int i = 0; i < tools.Length; i++) {
            if (tools[i].ToolType == type) {
                index = i;
                return true;
            }
        }

        index = -1;
        return false;
    }
}
