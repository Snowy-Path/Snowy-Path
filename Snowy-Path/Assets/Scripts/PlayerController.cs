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

    [Header("Camera")]
    [SerializeField] float lookSpeed = 2.0f;
    [SerializeField] float lookXLimit = 45.0f;

    private CharacterController controller;
    private bool canMove = true;
    private bool isGrounded = true;
    private bool isRunning;

    private float currentSpeed = 0f;
    private Vector3 inputs = Vector3.zero;
    private Vector3 xyVelocity = Vector3.zero;
    private Vector3 yVelocity = Vector3.zero;

    private Vector2 lookPos = Vector2.zero;
    private float xRotation = 0f;

    #region MONOBEHAVIOUR METHODS

    void Start() {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {

        //Update ground status
        isGrounded = Physics.CheckSphere(groundChecker.position, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);

        UpdateVelocity();
        ApplyGravity();
        Look();

        //Move
        if (canMove) {
            controller.Move(xyVelocity * Time.fixedDeltaTime);
        }

        controller.Move(yVelocity * Time.fixedDeltaTime);
    }
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
    //TODO : Replace by Stat
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
            if (isRunning && inputs.z > 0.5f) {
                currentSpeed = runningSpeed;
            }
            else {
                currentSpeed = walkingSpeed;
            }
        }

        zSpeed = inputs.z * currentSpeed * speedFactor;
        xSpeed = inputs.x * currentSpeed * speedFactor;
        xyVelocity = (forward * zSpeed) + (right * xSpeed);
    }

    private void UpdateInputs(Vector2 contextInputs) {
        //Update Axis
        inputs.x = contextInputs.x;
        inputs.z = contextInputs.y;
        //inputs.Normalize();
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
        isRunning = run;
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


    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckRadius);
    }
}
