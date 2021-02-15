using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
    [Header("Set up")]
    [SerializeField] Camera playerCamera;
    [SerializeField] Transform groundChecker;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;             //Temp
    [SerializeField] float gravity = -9.81f;

    [Header("Movement")]
    [SerializeField] float walkingSpeed = 7.5f;
    [SerializeField] float backwardSpeed = 7.5f;
    [SerializeField] float runningSpeed = 10f;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float maxSprintDuration = 6f;

    [Header("Camera")]
    [SerializeField] float lookSpeed = 2.0f;
    [SerializeField] float lookXLimit = 45.0f;

    //Status
    private CharacterController controller;
    private bool canMove = true;
    private bool isGrounded = true;
    private bool isRunning = false;

    //Sprint
    private float sprintTimer = 0f;
    private float sprintRecoveryTimer = 0f;
    private const float sprintTimeToRegen = 1f;
    private const float sprintRecoveryRate = 0.5f;

    //Velocity
    private float currentSpeed = 0f;
    private Vector3 inputs = Vector3.zero;
    private Vector3 xyVelocity = Vector3.zero;
    private Vector3 yVelocity = Vector3.zero;

    //Look
    private Vector2 lookPos = Vector2.zero;
    private float xRotation = 0f;

    //Parameters
    private const float inputThreshold = 0.2f;

    #region MONOBEHAVIOUR METHODS

    void Start() {
        controller = GetComponent<CharacterController>();

        //Lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {

        //Update ground status
        isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);

        //Process movement
        UpdateVelocity();
        ApplyGravity();
        Look();

        //Update stamina
        if (isRunning && inputs.z >= inputThreshold) {
            sprintTimer += Time.deltaTime;
            sprintRecoveryTimer = 0.0f;

            //Stop sprint if reached max sprint duration
            if (sprintTimer >= maxSprintDuration)
                ToggleRun(false);
        }
        else if (sprintTimer > 0) {     //Sprint recovery
            if (sprintRecoveryTimer >= sprintTimeToRegen) {
                sprintTimer = Mathf.Clamp(sprintTimer - (sprintRecoveryRate * Time.deltaTime), 0.0f, maxSprintDuration);
            }
            else
                sprintRecoveryTimer += Time.deltaTime;
        }

        #region DEBUG
        Keyboard keyboard = Keyboard.current;
        if (keyboard.kKey.wasPressedThisFrame)
        {
            AlterateSpeed(0.5f);
        }
        if (keyboard.jKey.wasPressedThisFrame)
        {
            AlterateSpeed(1f);
        }
        //if (keyboard.wKey.wasPressedThisFrame) {
        //    isMoving = true;
        //    starPos = transform.position;
        //}
        //else if(keyboard.wKey.wasReleasedThisFrame) {
        //    float dist = (transform.position - starPos).magnitude;
        //    float speed = dist / moveTestTimer;
        //    float factor = speed / runningSpeed;
        //    Debug.Log($"Dist={dist}  Time={moveTestTimer}  Speed={speed}  CurrentSpeed={currentSpeed}  Factor={factor}");

        //    isMoving = false;
        //    moveTestTimer = 0;
        //}

        //if(isMoving) {
        //    moveTestTimer += Time.deltaTime;
        //}
        #endregion

        //Move
        if (canMove) {
            controller.Move(xyVelocity * Time.deltaTime);
        }
        controller.Move(yVelocity * Time.deltaTime);
    }
    #endregion

    #region DEBUG
    //Vector3 starPos;
    //float moveTestTimer = 0f;
    //bool isMoving = false;
    #endregion


    #region INPUTS SYSTEM EVENTS
    public void OnMove(InputAction.CallbackContext context) {
        UpdateInputs(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context) {
        lookPos = context.ReadValue<Vector2>();
    }

    public void OnStartRun(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            ToggleRun(true);
        }
    }

    public void OnStopRun(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            ToggleRun(false);
        }
    }

    public void OnJump(InputAction.CallbackContext context) {
        if (context.phase == InputActionPhase.Performed) {
            Jump();
        }
    }
    #endregion

    #region PUBLIC METHODS
    //TODO : Replace by Stat ?
    private float speedFactor = 1f;
    public void AlterateSpeed(float factor) {
        speedFactor = factor;
    }
    #endregion

    #region PRIVATE METHODS
    private void UpdateVelocity() {

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        float xSpeed = 0f;
        float zSpeed = 0f;

        //Check for speed change if player is on ground
        if (isGrounded) {
            if (isRunning && inputs.z >= inputThreshold) {
                currentSpeed = runningSpeed;
            }
            else if (inputs.z <= -inputThreshold) {
                currentSpeed = backwardSpeed;
            }
            else {
                currentSpeed = walkingSpeed;
            }
        }

        //Compute x and z speed
        zSpeed = inputs.z * currentSpeed * speedFactor;
        xSpeed = inputs.x * currentSpeed * speedFactor;
        xyVelocity = (forward * zSpeed) + (right * xSpeed);
    }

    private void UpdateInputs(Vector2 contextInputs) {
        //Update Axis
        inputs.x = contextInputs.x;
        inputs.z = contextInputs.y;
    }

    private void ApplyGravity() {
        //Reduce gravity if grounded
        if (isGrounded && yVelocity.y < 0)
            yVelocity.y = -2f;

        //Apply gravity
        yVelocity.y += gravity * Time.deltaTime;
    }

    private void Jump() {
        if (isGrounded) {
            yVelocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    private void ToggleRun(bool run) {
        if (sprintTimer < maxSprintDuration) {
            isRunning = run;
        }
        else
            isRunning = false;
    }

    private void Look() {
        if (canMove) {
            xRotation += -lookPos.y * lookSpeed;
            xRotation = Mathf.Clamp(xRotation, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            transform.rotation *= Quaternion.Euler(0, lookPos.x * lookSpeed, 0);
        }
    }
    #endregion

    #region DEBUG
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckRadius);
    }

    private void OnGUI() {
        GUI.Label(new Rect(50, 400, 400, 200), $"Sprint duration : {sprintTimer}");
        GUI.Label(new Rect(50, 350, 400, 200), $"CurrentSpeed : {currentSpeed}");
    }
    #endregion
}
