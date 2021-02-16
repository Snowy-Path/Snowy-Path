using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputTest : MonoBehaviour {

    PlayerInput playerInput;
    ActionMap inputType = ActionMap.Gameplay;

    void Start() {
        playerInput = GetComponent<PlayerInput>();
    }

    enum Tool {
        Map,
        Telescope,
        Gun,
        COUNT
    }

    enum ActionMap {
        Gameplay,
        Map,
        Stamp
    }

    private void ChangeActionMap(ActionMap newMap) {
        inputType = newMap;

        switch (inputType) {
            case ActionMap.Gameplay:
                playerInput.SwitchCurrentActionMap("Gameplay");
                break;
            case ActionMap.Map:
                playerInput.SwitchCurrentActionMap("Map");
                break;
            case ActionMap.Stamp:
                playerInput.SwitchCurrentActionMap("Stamp");
                break;
        }
    }


    #region Gameplay

    private Vector2 move;
    private Vector2 look;
    private bool running = false;
    private bool jumping = false; //Never reset in this test
    private bool interacting = false; //Never reset in this test
    private Tool tool = Tool.Map;


    public void OnMove(InputAction.CallbackContext context) {
        Debug.Log($"OnMove");
        move = context.ReadValue<Vector2>();
        //Insert action here
    }

    public void OnLook(InputAction.CallbackContext context) {
        Debug.Log($"OnLook");
        look = context.ReadValue<Vector2>();
        //Insert action here
    }

    public void OnInteract(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnInteract");
            interacting = true;
            //Insert action here
        }
    }

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

    public void OnLeftHandMain(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnLeftHandMain");
            //Insert action here

            if (tool == Tool.Map) {
                ChangeActionMap(ActionMap.Map);
            }

        }
    }

    public void OnLeftHandSecondary(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnLeftHandSecondary");
            //Insert action here
        }
    }

    public void OnRightHandMain(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnRightHandMain");
            //Insert action here
        }
    }

    public void OnRightHandSecondary(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnRightHandSecondary");
            //Insert action here
        }
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnJump");
            jumping = true;
            //Insert action here
        }
    }

    public void OnStartRun(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnStartRun");
            running = true;
            //Insert action here
        }
    }

    public void OnStopRun(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnStopRun");
            running = false;
            //Insert action here
        }
    }

    public void OnSelectMap(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnSelectMap");
            tool = Tool.Map;
            //Insert action here
        }
    }

    public void OnSelectTelescope(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnSelectTelescope");
            tool = Tool.Telescope;
            //Insert action here
        }
    }

    public void OnSelectGun(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnSelectGun");
            tool = Tool.Gun;
            //Insert action here
        }
    }

    #endregion



    #region Map

    private Vector2 mapMove;
    private float mapZoom;

    public void OnBackToGame(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnBackToGame");
            ChangeActionMap(ActionMap.Gameplay);
            //Insert action here
        }
    }

    public void OnApplyStamp(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnApplyStamp");
            ChangeActionMap(ActionMap.Stamp);
            //Insert action here
        }
    }

    public void OnMapMove(InputAction.CallbackContext context) {
        Debug.Log($"OnMapMove");
        mapMove = context.ReadValue<Vector2>();
        //Insert action here
    }

    public void OnMapZoom(InputAction.CallbackContext context) {
        Debug.Log($"OnMapZoom");
        mapZoom = context.ReadValue<float>();
        //Insert action here
    }


    #endregion



    #region Stamp

    public void OnDrawValidate(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnDrawValidate");
            ChangeActionMap(ActionMap.Map);
            //Insert action here
        }
    }

    public void OnErase(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnErase");
            ChangeActionMap(ActionMap.Map);
            //Insert action here
        }
    }

    #endregion


    public void OnPause(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Debug.Log($"OnPause");
            //Insert action here
        }
    }


    #region GUI

    private void OnGUI() {


        switch (inputType) {
            case ActionMap.Gameplay:
                GUI.Box(new Rect(10, 10, 160, 220), "Gameplay");

                GUI.Label(new Rect(20, 40, 140, 30), $"Move : {move}");
                GUI.Label(new Rect(20, 70, 140, 30), $"Look : {look}");
                GUI.Label(new Rect(20, 100, 140, 30), $"Tool : {tool}");

                GUI.Label(new Rect(20, 130, 140, 30), $"Running : {running}");
                GUI.Label(new Rect(20, 160, 140, 30), $"Jumping : {jumping}");
                GUI.Label(new Rect(20, 190, 140, 30), $"Interacting : {interacting}");
                break;

            case ActionMap.Map:
                GUI.Box(new Rect(10, 10, 160, 100), "Map");

                GUI.Label(new Rect(20, 40, 140, 30), $"MapMove : {mapMove}");
                GUI.Label(new Rect(20, 70, 140, 30), $"MapZoom : {mapZoom}");
                break;

            case ActionMap.Stamp:
                GUI.Box(new Rect(10, 10, 160, 220), "Stamp");
                break;
        }


    }

    #endregion

}
