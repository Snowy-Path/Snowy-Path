using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;

public class HandController : MonoBehaviour {

    // Variables for references of the different tools
    public GameObject gun;
    public GameObject telescope;
    public GameObject compass;


    public IHandTool CurrentTool {
        get {
            if (currentToolIndex >= 0) {
                return tools[currentToolIndex];
            }
            else
                return null;
        }
    }

    private Animator handAnimator;
    private IHandTool[] tools;
    private int currentToolIndex = 0;
    private int toolToEquip;
    private bool canUse = true;

    // Start is called before the first frame update
    void Start() {
        handAnimator = GetComponent<Animator>();
        tools = GetComponentsInChildren<IHandTool>(true);
        foreach (var tool in tools) {
            tool.handAnimator = handAnimator;
        }
        toolToEquip = currentToolIndex;
        DisplayCurrentTool();
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
        if (context.phase == InputActionPhase.Performed) {
            // NOTE: Could be used later if LDs decide that the cursor should be shown by default in when using a controller
            // bool isKeyboard = context.control.device is Keyboard;
            // bool isGamepad = context.control.device is Gamepad;
            // Debug.Log($"Keyboard: {isKeyboard}, Gamepad: {isGamepad}");

            SecondaryUseCurrentTool();
        }
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
        if (tools.Length > 0 && canUse)
            tools[currentToolIndex].StartPrimaryUse();
    }

    /// <summary>
    /// Use active tool secondary function
    /// </summary>
    private void SecondaryUseCurrentTool() {
        if (tools.Length > 0 && canUse)
            tools[currentToolIndex].SecondaryUse();
    }


    /// <summary>
    /// Cancel active tool use
    /// </summary>
    private void CancelUseCurrentTool() {
        if (tools.Length > 0 && canUse)
            tools[currentToolIndex].CancelPrimaryUse();
    }

    /// <summary>
    /// Switch up or down current tool index then equip corresponding tool
    /// </summary>
    /// <param name="indexShift">Number of tools to shift</param>
    private void SwitchTool(int indexShift) {
        //Guard : if timer is not reset OR there is no tools, return
        if (tools.Length == 0 || (CurrentTool != null && CurrentTool.IsBusy))
            return;

        //shift the current index
        toolToEquip += indexShift;

        //Handle index outside the array
        if (toolToEquip >= tools.Length) {
            toolToEquip = 0;
        }
        else if (toolToEquip < 0) {
            toolToEquip = tools.Length - 1;
        }

        if (canUse) // = if not in swapping anim
            LaunchSwapAnimation();
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
        //if (handAnimator.GetCurrentAnimatorStateInfo(0).IsName("SwitchLeftTool")) {
        //    return false;
        //}

        if (!CurrentTool.IsBusy && TryGetToolIndex(type, out int index) && currentToolIndex != index) {
            toolToEquip = index;

            LaunchSwapAnimation();
            return true;
        }
        else
            return false;
    }

    private void LaunchSwapAnimation() {
        //Update display
        CancelUseCurrentTool();
        handAnimator.SetTrigger("SwitchTool");
        tools[currentToolIndex].IsBusy = false;
        canUse = false;
    }

    public void DisplayCurrentTool() {
        currentToolIndex = toolToEquip;
        HideTools();
        tools[currentToolIndex].ToggleDisplay(true);
        canUse = true;
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
