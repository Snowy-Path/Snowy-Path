using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HQBindingDisplay : MonoBehaviour
{

    [SerializeField] private InputActionReference Action;
    private PlayerInput playerInput;
    [SerializeField] private TMP_Text bindingDisplayNameText = null;
    [SerializeField] private GameObject startRebindObject = null;
    [SerializeField] private GameObject waitingForInputObjct = null;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    private void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    /// <summary>
    /// Rebinding method, switch Action map 
    /// </summary>
    public void StartRebinding()
    {
        startRebindObject.SetActive(false);
        waitingForInputObjct.SetActive(true);

        playerInput.SwitchCurrentActionMap("Rebindingkeys");

        rebindingOperation = Action.action.PerformInteractiveRebinding()
             .OnMatchWaitForAnother(0.1f)
             .OnComplete(operation => RebindComplete())
             .Start();

    }

    private void RebindComplete()
    {
        int bindingIndex = Action.action.GetBindingIndexForControl(Action.action.controls[0]);
        
        bindingDisplayNameText.text = InputControlPath.ToHumanReadableString(
            Action.action.bindings[0].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice );

        rebindingOperation.Dispose();

        startRebindObject.SetActive(true);
        waitingForInputObjct.SetActive(false);

        playerInput.SwitchCurrentActionMap("Gameplay");
    }
}