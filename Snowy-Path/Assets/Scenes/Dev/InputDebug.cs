using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Used in the Prefab InputDebug.
/// Bind every event of every Action Map in the InputAction object.
/// Either prints on console/debug output OR show on-screen values if needed.
/// Linked to the PlayerController and the InteractionController to retrieves more value and act as a debug overlay for them.
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class InputDebug : MonoBehaviour {

    #region Common Variables
    private PlayerInput playerInput;
    [Tooltip("The PlayerController script of the player Prefab.")]
    public PlayerController playerControl;
    [Tooltip("The InteractionController script of the player Prefab.")]
    public InteractionController interactControl;
    #endregion

    private ActionMap inputType = ActionMap.Gameplay;

    void Awake() {
        playerInput = GetComponent<PlayerInput>();
    }

    /// <summary>
    /// Simple enum to tools.
    /// Used ONLY in this script.
    /// </summary>
    private enum Tool {
        Map,
        Telescope,
        Gun,
        COUNT
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
    private bool running = false;
    private bool jumping = false; //Never reset in this test
    private bool interacting = false; //Never reset in this test
    private Tool tool = Tool.Map;

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
            interacting = true;
            //Insert action here
        }
    }

    /// <summary>
    /// SwitchTool action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnSwitchTools(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnSwitchTools");

            Tool nextTool = tool + 1;
            if (nextTool == Tool.COUNT) {
                nextTool = Tool.Map;
            }
            tool = nextTool;

            Debug.Log(tool);
        }
    }

    /// <summary>
    /// LeftHandMain action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnLeftHandMain(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnLeftHandMain");
            //Insert action here

            if (tool == Tool.Map) {
                ChangeActionMap(ActionMap.Map);
            }

        }
    }

    /// <summary>
    /// LeftHandSecondary action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnLeftHandSecondary(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnLeftHandSecondary");
            //Insert action here
        }
    }

    /// <summary>
    /// RightHandMain action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnRightHandMain(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnRightHandMain");
            //Insert action here
        }
    }

    /// <summary>
    /// RightHandSecondary action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnRightHandSecondary(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnRightHandSecondary");
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
            jumping = true;
            //Insert action here
        }
    }

    /// <summary>
    /// SprintStart action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnStartRun(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnStartRun");
            running = true;
            //Insert action here
        }
    }

    /// <summary>
    /// SprintStart action callback.
    /// </summary>
    /// <param name="context">Contains input values.</param>
    public void OnStopRun(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnStopRun");
            running = false;
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
            tool = Tool.Map;
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
            tool = Tool.Telescope;
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
            tool = Tool.Gun;
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
                GUI.Box(new Rect(10, 10, 200, 370), "Gameplay");

                GUI.Label(new Rect(20, 40, 180, 30), $"Move : {move}");
                GUI.Label(new Rect(20, 70, 180, 30), $"Look : {look}");
                GUI.Label(new Rect(20, 100, 180, 30), $"Tool : {tool}");

                GUI.Label(new Rect(20, 130, 180, 30), $"CanInteract : {interactControl.CanInteract()}");

                GUI.Label(new Rect(20, 160, 180, 30), $"Sprint duration : {playerControl.SprintTimer}");
                GUI.Label(new Rect(20, 190, 180, 30), $"Sprint Reco T : {playerControl.SprintRecoveryTimer}");
                GUI.Label(new Rect(20, 220, 180, 30), $"CurrentSpeed : {playerControl.CurrentSpeed}");
                GUI.Label(new Rect(20, 250, 180, 30), $"G Velocity : {playerControl.XZVelocity + playerControl.YVelocity}");
                GUI.Label(new Rect(20, 280, 180, 30), $"Air Velocity : {playerControl.AirVelocity}");

                GUI.Label(new Rect(20, 310, 180, 30), $"Running : {playerControl.IsRunning}");
                GUI.Label(new Rect(20, 340, 180, 30), $"Grounded : {playerControl.IsGrounded}");
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
