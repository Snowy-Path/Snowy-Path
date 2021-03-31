using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class HQBindingDisplay : MonoBehaviour
{
    [SerializeField] private InputActionReference Action;
    [SerializeField] private PlayerInput playerInput = null;
    [SerializeField] private TMP_Text bindingDisplayNameText = null;
    [SerializeField] private GameObject startRebindObject = null;
    [SerializeField] private GameObject waitingForInputObjct = null;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    private const string RebindsKey = "rebinds";
    //private void Start()
    //{
    //    string rebinds = PlayerPrefs.GetString(RebindsKey, string.Empty);

    //    if (string.IsNullOrEmpty(rebinds)) { return; }

    //    //playerInput.actions.LoadBindingOverridesFromJson(rebinds); 
    //}
    //public void Save()
    //{
    //    //string rebinds = playerInput.actions.SaveBindingOverridesAsJson();

    //    PlayerPrefs.SetString("rebinds", rebinds);
    //}

    public void StartRebinding()
    {
        startRebindObject.SetActive(false);
        waitingForInputObjct.SetActive(true);

        playerInput.SwitchCurrentActionMap("Rebindingkeys");

        rebindingOperation = Action.action.PerformInteractiveRebinding()
             .WithControlsExcluding("Mouse")
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