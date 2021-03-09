using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

/// <summary>
/// Used in the Prefab InputDebug.
/// Bind every event of every Action Map in the InputAction object.
/// Either prints on console/debug output OR show on-screen values if needed.
/// Linked to the PlayerController and the InteractionController to retrieves more value and act as a debug overlay for them.
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class InputDebug : MonoBehaviour {

    /*
    Changed to lastest input for both controller & keyboard/mouse.
    Fixed attack animation starting multiple times in a row.
    */

    #region Common Variables

    private PlayerInput playerInput;

    [Tooltip("The PlayerController script of the player Prefab.")]
    public PlayerController playerControl;

    [Tooltip("The InteractionController script of the player Prefab.")]
    public InteractionController interactControl;

    [Tooltip("The HandController script of the player Prefab.")]
    public HandController handControl;
    #endregion

    private ActionMap inputType = ActionMap.Gameplay;

    void Awake() {
        playerInput = GetComponent<PlayerInput>();
    }

    /// <summary>
    /// Simple enum to switch ActionMap.
    /// Used ONLY in this script.
    /// </summary>
    private enum ActionMap {
        UI,
        Gameplay,
        Map,
        Stamp
    }

    private void ChangeActionMap(ActionMap newMap) {
        inputType = newMap;

        switch (inputType) {
            case ActionMap.UI:
                playerInput.SwitchCurrentActionMap("UI");
                playerControl.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
                break;

            case ActionMap.Gameplay:
                playerInput.SwitchCurrentActionMap("Gameplay");
                playerControl.GetComponent<PlayerInput>().SwitchCurrentActionMap("Gameplay");
                break;

            case ActionMap.Map:
                playerInput.SwitchCurrentActionMap("Map");
                playerControl.GetComponent<PlayerInput>().SwitchCurrentActionMap("Map");
                break;

            case ActionMap.Stamp:
                playerInput.SwitchCurrentActionMap("Stamp");
                playerControl.GetComponent<PlayerInput>().SwitchCurrentActionMap("Stamp");
                break;
        }
    }


    #region Gameplay

    // Gameplay Variables
    private Vector2 move;
    private Vector2 look;

    /// <summary>
    /// Move action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnMove(InputAction.CallbackContext context) {
        Debug.Log($"OnMove");
        move = context.ReadValue<Vector2>();
        //Insert action here
    }

    /// <summary>
    /// Look action callback.
    /// </summary>
    /// <param name="context"></param>
    public void OnLook(InputAction.CallbackContext context) {
        Debug.Log($"OnLook");
        look = context.ReadValue<Vector2>();
        //Insert action here
    }

    /// <summary>
    /// Interact action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnInteract(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnInteract");
            //Insert action here
        }
    }

    /// <summary>
    /// Main tool use action callback.
    /// Use Started & Canceled CallbackContext to simulate both hold & press action with the same binding.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnMainTool(InputAction.CallbackContext context) {

        Debug.Log($"OnMainTool : {context.phase}");

        switch (context.phase) {

            case InputActionPhase.Started:
                // Call Interface "START HOLD"
                // Call Interface "MAKE PRESS"
                break;

            case InputActionPhase.Canceled:
                // Call Interface "STOP HOLD"
                break;

        }

    }

    /// <summary>
    /// Secondary tool use action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnSecondaryTool(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnSecondaryTool");
            //Insert action here
        }
    }

    /// <summary>
    /// Attack action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnAttack(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnAttack");
            //Insert action here
        }
    }

    /// <summary>
    /// Jump action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnJump(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnJump");
            //Insert action here
        }
    }

    /// <summary>
    /// Hold sprint action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnHoldSprint(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnHoldSprint");
            //Insert action here
        }
    }

    /// <summary>
    /// Toggle sprint action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnToggleSprint(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnToggleSprint");
            //Insert action here
        }
    }

    /// <summary>
    /// SelectMap action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnSelectMap(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnSelectMap");
            //Insert action here
        }
    }

    /// <summary>
    /// SelectTelescope action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnSelectTelescope(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnSelectTelescope");
            //Insert action here
        }
    }

    /// <summary>
    /// SelectGun action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnSelectGun(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnSelectGun");
            //Insert action here
        }
    }

    /// <summary>
    /// Galerie action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnGalerie(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnGalerie");
            //Insert action here
        }
    }

    /// <summary>
    /// SwitchTools action callback.
    /// </summary>
    /// <param name="context"></param>
    public void OnSwitchTools(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            int res = (int)context.ReadValue<float>();
            Debug.Log($"OnSwitchTools : {res}");
            //Insert action here

        }
    }

    #endregion



    #region Map

    // Map movement variables
    private Vector2 mapMove;
    private float mapZoom;

    /// <summary>
    /// BackToGame action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnBackToGame(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnBackToGame");
            ChangeActionMap(ActionMap.Gameplay);
            //Insert action here
        }
    }

    /// <summary>
    /// ApplyStamp action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnApplyStamp(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnApplyStamp");
            ChangeActionMap(ActionMap.Stamp);
            //Insert action here
        }
    }

    /// <summary>
    /// MapMove action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnMapMove(InputAction.CallbackContext context) {
        Debug.Log($"OnMapMove");
        mapMove = context.ReadValue<Vector2>();
        //Insert action here
    }

    /// <summary>
    /// MapZoom action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnMapZoom(InputAction.CallbackContext context) {
        Debug.Log($"OnMapZoom");
        mapZoom = context.ReadValue<float>();
        //Insert action here
    }


    #endregion



    #region Stamp

    /// <summary>
    /// DrawValidate action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnDrawValidate(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnDrawValidate");
            ChangeActionMap(ActionMap.Map);
            //Insert action here
        }
    }

    /// <summary>
    /// Erase action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnErase(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnErase");
            ChangeActionMap(ActionMap.Map);
            //Insert action here
        }
    }

    #endregion


    /// <summary>
    /// Pause action callback.
    /// Must be bind to every Pause actions.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnPause(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnPause");
            Time.timeScale = 0f;
            ChangeActionMap(ActionMap.UI);
            //Insert action here
        }
    }



    #region UI

    /// <summary>
    /// Resume action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnResume(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnResume");
            Time.timeScale = 1f;
            ChangeActionMap(ActionMap.Gameplay);
            //Insert action here
        }
    }

    #endregion


    #region GUI

    /// <summary>
    /// On-screen GUI for debug purposes.
    /// Depending on the ActionMap currently bind, it displays corresponding informations.
    /// </summary>
    private void OnGUI() {

        switch (inputType) {

            case ActionMap.UI:
                GUI.Box(new Rect(10, 10, 200, 70), "UI");

                GUI.Label(new Rect(20, 40, 180, 30), $"Nothing to show ...");
                break;

            case ActionMap.Gameplay:
                GUI.Box(new Rect(10, 10, 200, 340), "Gameplay");

                GUI.Label(new Rect(20, 40, 180, 30), $"Move : {move}");
                GUI.Label(new Rect(20, 70, 180, 30), $"Look : {look}");
                //GUI.Label(new Rect(20, 100, 180, 30), $"Tool : {tool}");

                GUI.Label(new Rect(20, 130, 180, 30), $"CanInteract : {interactControl.CanInteract()}");

                GUI.Label(new Rect(20, 160, 180, 30), $"Sprint duration : {playerControl.SprintTimer}");
                GUI.Label(new Rect(20, 190, 180, 30), $"CurrentSpeed : {playerControl.CurrentSpeed}");
                GUI.Label(new Rect(20, 220, 180, 30), $"G Velocity : {playerControl.XZVelocity + playerControl.YVelocity}");
                GUI.Label(new Rect(20, 250, 180, 30), $"Actual Velocity : {playerControl.ActualVelocity}");

                GUI.Label(new Rect(20, 300, 180, 30), $"Running : {playerControl.IsRunning}");
                GUI.Label(new Rect(20, 320, 180, 30), $"Grounded : {playerControl.IsGrounded}");
                break;

            case ActionMap.Map:
                GUI.Box(new Rect(10, 10, 200, 100), "Map");

                GUI.Label(new Rect(20, 40, 180, 30), $"MapMove : {mapMove}");
                GUI.Label(new Rect(20, 70, 180, 30), $"MapZoom : {mapZoom}");
                break;

            case ActionMap.Stamp:
                GUI.Box(new Rect(10, 10, 200, 70), "Stamp");

                GUI.Label(new Rect(20, 40, 180, 30), $"Nothing to show ...");
                break;
        }


    }

    #endregion

}
